using UnityEngine;
using System.Collections.Generic;

public class Towers : MonoBehaviour
{
    [SerializeField] private TowerDatas data;  // template

    private TowerRuntimeData runtimeData;      // runtime stats for this tower
    private CircleCollider2D circleCollider;
    private List<Enemies> enemiesInRange;
    private ObjPool bulletPool;
    private float shootTimer;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        Enemies.OnEnemyKilled += EnemyGone;
    }

    private void OnDisable()
    {
        Enemies.OnEnemyKilled -= EnemyGone;
    }

    private void Awake()
    {
        // Copy template into runtime data
        runtimeData = new TowerRuntimeData(data);

        circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider == null)
            circleCollider = gameObject.AddComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;
        circleCollider.radius = runtimeData.range / transform.lossyScale.x;

        enemiesInRange = new List<Enemies>();
        bulletPool = GetComponent<ObjPool>();
        shootTimer = runtimeData.attackDelay;

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Shooting logic
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            shootTimer = runtimeData.attackDelay;
            Shoot();
        }
    }

    private void OnDrawGizmos()
    {
        if (runtimeData != null)
            Gizmos.DrawWireSphere(transform.position, runtimeData.range);
        else if (data != null)
            Gizmos.DrawWireSphere(transform.position, data.range);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemies enemy = collision.GetComponent<Enemies>();
            if (!enemiesInRange.Contains(enemy))
                enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemies enemy = collision.GetComponent<Enemies>();
            enemiesInRange.Remove(enemy);
        }
    }

    private void Shoot()
    {
        enemiesInRange.RemoveAll(e => e == null || !e.gameObject.activeInHierarchy);
        if (enemiesInRange.Count == 0) return;

        if (animator != null)
            animator.SetTrigger("Attack");

        FireProjectile();
    }

    public void FireProjectile()
    {
        enemiesInRange.RemoveAll(e => e == null || !e.gameObject.activeInHierarchy);
        if (enemiesInRange.Count == 0) return;

        GameObject bullet = bulletPool.GetPObj();
        bullet.transform.position = transform.position;
        bullet.SetActive(true);

        Vector2 shootDirection = (enemiesInRange[0].transform.position - transform.position).normalized;
        bullet.GetComponent<Bullet>().Shoot(runtimeData, shootDirection);
    }

    private void EnemyGone(Enemies enemy)
    {
        enemiesInRange.Remove(enemy);
    }

    // -----------------------------
    // Upgrade method with coins & max level
    // -----------------------------
    public void UpgradeTower()
    {
        // Check max level
        if (runtimeData.level >= runtimeData.maxLevel)
        {
            if (UI.instance != null)
                StartCoroutine(UI.instance.ShowWarning("Tower is already at max level!"));
            return;
        }

        // Check coins
        if (GManager.instance.loots < runtimeData.upgradeCost)
        {
            if (UI.instance != null)
                StartCoroutine(UI.instance.ShowWarning("Not Enough Coins!"));
            return;
        }

        // Spend coins
        GManager.instance.spendmoney(runtimeData.upgradeCost);

        // Upgrade stats
        runtimeData.Upgrade();
        circleCollider.radius = runtimeData.range / transform.lossyScale.x;
        shootTimer = runtimeData.attackDelay;

        // Increase upgrade cost for next level
        runtimeData.upgradeCost = Mathf.RoundToInt(runtimeData.upgradeCost * runtimeData.upgradeCostMultiplier);

        // Show upgrade message
        if (UI.instance != null)
            StartCoroutine(UI.instance.ShowWarning(
                $"Tower Upgraded to Level {runtimeData.level}!\n"
            ));
    }

}
