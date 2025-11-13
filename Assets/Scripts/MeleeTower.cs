using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class MeleeTower : MonoBehaviour
{
    [Header("Template")]
    [SerializeField] private TowerDatas data;  // ScriptableObject template

    public TowerRuntimeData runtimeData;      // runtime copy for upgrades
    private ObjPool bulletpool;
    private float shootTimer;

    [Header("Tack Shooter Settings")]
    [SerializeField] private int tackCount = 8;
    [SerializeField] private float tackSpread = 360f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Colliders")]
    [SerializeField] private CircleCollider2D rangeCollider; // expands with upgrades

    private List<Enemies> enemiesInRange = new List<Enemies>();

    public TMP_Text levelLabel;
    private void Awake()
    {
        runtimeData = new TowerRuntimeData(data); // copy template into runtime
    }

    private void Start()
    {
        bulletpool = GetComponent<ObjPool>();
        shootTimer = runtimeData.attackDelay;

        if (animator == null)
            animator = GetComponent<Animator>();

        if (rangeCollider == null)
            rangeCollider = gameObject.AddComponent<CircleCollider2D>();

        rangeCollider.isTrigger = true;
        rangeCollider.radius = runtimeData.range;

        if (levelLabel != null)
            levelLabel.text = $"LVL {runtimeData.level}";   
    }

    private void Update()
    {
        enemiesInRange.RemoveAll(e => e == null || !e.gameObject.activeInHierarchy);
        if (enemiesInRange.Count == 0)
            return;

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            shootTimer = runtimeData.attackDelay;
            ShootBurst();
        }
    }

    private void ShootBurst()
    {
        if (animator != null)
            animator.SetTrigger("Attack");
    }

    // Called from animation event
    public void Fire()
    {
        float angleStep = tackSpread / tackCount;
        float currentAngle = 0f;

        for (int i = 0; i < tackCount; i++)
        {
            GameObject bullet = bulletpool.GetPObj();
            bullet.transform.position = transform.position;
            bullet.SetActive(true);

            Vector2 shootDir = Quaternion.Euler(0, 0, currentAngle) * Vector2.up;
            bullet.GetComponent<Bullet>().Shoot(runtimeData, shootDir);

            currentAngle += angleStep;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemies e = collision.GetComponent<Enemies>();
            if (e != null && !enemiesInRange.Contains(e))
                enemiesInRange.Add(e);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemies e = collision.GetComponent<Enemies>();
            enemiesInRange.Remove(e);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (runtimeData != null)
            Gizmos.DrawWireSphere(transform.position, runtimeData.range);
        else if (data != null)
            Gizmos.DrawWireSphere(transform.position, data.range);
    }

    // Upgrade this tower
    public void UpgradeTower()
    {
        if (runtimeData.level >= runtimeData.maxLevel)
        {
            if (UI.instance != null)
                StartCoroutine(UI.instance.ShowWarning("Tower is already at max level!"));
            return;
        }

        if (GManager.instance.loots < runtimeData.upgradeCost)
        {
            if (UI.instance != null)
                StartCoroutine(UI.instance.ShowWarning("Not Enough Coins!"));
            return;
        }

        // Spend money
        GManager.instance.spendmoney(runtimeData.upgradeCost);

        // Upgrade stats
        runtimeData.Upgrade();
        rangeCollider.radius = runtimeData.range;
        shootTimer = runtimeData.attackDelay;

        runtimeData.upgradeCost = Mathf.RoundToInt(runtimeData.upgradeCost * runtimeData.upgradeCostMultiplier);

        // Show upgrade message
        if (UI.instance != null)
            StartCoroutine(UI.instance.ShowWarning(
                $"Tower Upgraded to Level {runtimeData.level}!\n"
            ));

        if (levelLabel != null)
            levelLabel.text = $"LVL {runtimeData.level}";
    }




}
