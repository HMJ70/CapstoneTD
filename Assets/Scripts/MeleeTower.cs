using UnityEngine;
using System.Collections.Generic;

public class MeleeTower : MonoBehaviour
{
    [SerializeField] private TowerDatas data;

    private ObjPool bulletpool;
    private float shootTimer;

    [Header("Tack Shooter Settings")]
    [SerializeField] private int tackCount = 8;
    [SerializeField] private float tackSpread = 360f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private CircleCollider2D rangeCollider;
    private List<Enemies> enemiesInRange = new List<Enemies>();

    private void Start()
    {
        bulletpool = GetComponent<ObjPool>();
        shootTimer = data.attackdelay;

        if (animator == null)
            animator = GetComponent<Animator>();

        rangeCollider = GetComponent<CircleCollider2D>();
        if (rangeCollider == null)
            rangeCollider = gameObject.AddComponent<CircleCollider2D>();

        rangeCollider.isTrigger = true;
        rangeCollider.radius = data.range;
    }

    private void Update()
    {
        enemiesInRange.RemoveAll(e => e == null || !e.gameObject.activeInHierarchy);
        if (enemiesInRange.Count == 0)
            return;

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            shootTimer = data.attackdelay;
            ShootBurst();
        }
    }

    private void ShootBurst()
    {
        // Play the attack animation, but don't shoot yet
        if (animator != null)
            animator.SetTrigger("Attack");
    }

    // Called from the animation event at the moment of impact
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
            bullet.GetComponent<Bullet>().Shoot(data, shootDir);

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
        Gizmos.DrawWireSphere(transform.position, data.range);
    }
}
