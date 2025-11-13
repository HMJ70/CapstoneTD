using UnityEngine;
using System.Collections.Generic;

public class Towers : MonoBehaviour
{
    [SerializeField] private TowerDatas data;  // template

    private TowerRuntimeData runtimeData;      // runtime stats for this tower
    private CircleCollider2D circleCollider;
    private List<Enemies> enemiesinrange;
    private ObjPool bulletpool;
    private float shootime;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        Enemies.OnEnemyKilled += Enemygone;
    }

    private void OnDisable()
    {
        Enemies.OnEnemyKilled -= Enemygone;
    }

    private void Awake()
    {
        // Step 2a: copy template into runtime data
        runtimeData = new TowerRuntimeData(data);

        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = runtimeData.range / transform.lossyScale.x;

        enemiesinrange = new List<Enemies>();
        bulletpool = GetComponent<ObjPool>();
        shootime = runtimeData.attackDelay;

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        shootime -= Time.deltaTime;
        if (shootime <= 0)
        {
            shootime = runtimeData.attackDelay;
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
            enemiesinrange.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemies enemy = collision.GetComponent<Enemies>();
            enemiesinrange.Remove(enemy);
        }
    }

    private void Shoot()
    {
        enemiesinrange.RemoveAll(e => e == null || !e.gameObject.activeInHierarchy);

        if (enemiesinrange.Count > 0)
        {
            if (animator != null)
                animator.SetTrigger("Attack");

            FireProjectile();
        }
    }

    public void FireProjectile()
    {
        enemiesinrange.RemoveAll(e => e == null || !e.gameObject.activeInHierarchy);

        if (enemiesinrange.Count == 0)
            return;

        GameObject Bullet = bulletpool.GetPObj();
        Bullet.transform.position = transform.position;
        Bullet.SetActive(true);

        Vector2 shootdirection = (enemiesinrange[0].transform.position - transform.position).normalized;

        // Step 2b: send runtimeData instead of template
        Bullet.GetComponent<Bullet>().Shoot(runtimeData, shootdirection);
    }

    private void Enemygone(Enemies enemies)
    {
        enemiesinrange.Remove(enemies);
    }

    // Step 2c: Upgrade method
    public void UpgradeTower()
    {
        runtimeData.Upgrade();
        circleCollider.radius = runtimeData.range / transform.lossyScale.x;
        Debug.Log($"Tower upgraded to level {runtimeData.level} | DMG: {runtimeData.dmg} | Range: {runtimeData.range}");
    }
    private void OnMouseDown()
    {
        // This method is called when the tower is clicked
        UpgradeTower();
    }

}
