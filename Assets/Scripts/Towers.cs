using UnityEngine;
using System.Collections.Generic;

public class Towers : MonoBehaviour
{
    [SerializeField] private TowerDatas data;

    private CircleCollider2D circleCollider;
    private List<Enemies> enemiesinrange;
    private ObjPool bulletpool;
    private float shootime;

    [Header("Animation")]
    [SerializeField] private Animator animator; // 👈 Added

    private void OnEnable()
    {
        Enemies.OnEnemyKilled += Enemygone;
    }

    private void OnDisable()
    {
        Enemies.OnEnemyKilled -= Enemygone;
    }

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = data.range / transform.lossyScale.x;

        enemiesinrange = new List<Enemies>();
        bulletpool = GetComponent<ObjPool>();
        shootime = data.attackdelay;

        // 👇 Automatically get Animator if not manually set
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        shootime -= Time.deltaTime;
        if (shootime <= 0)
        {
            shootime = data.attackdelay;
            Shoot();
        }
    }

    private void OnDrawGizmos()
    {
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
            if (enemiesinrange.Contains(enemy))
            {
                enemiesinrange.Remove(enemy);
            }
        }
    }

    private void Shoot()
    {
        enemiesinrange.RemoveAll(enemies => enemies == null || !enemies.gameObject.activeInHierarchy);

        if (enemiesinrange.Count > 0)
        {
            // ✅ Play attack animation
            if (animator != null)
                animator.SetTrigger("Attack");

            // 👇 Either shoot immediately (simple version)
            FireProjectile();
        }
    }

    // 🔥 Separate shooting logic (optional for animation event)
    public void FireProjectile()
    {
        // Remove null or inactive enemies
        enemiesinrange.RemoveAll(e => e == null || !e.gameObject.activeInHierarchy);

        // Check if we still have a valid target
        if (enemiesinrange.Count == 0)
            return; // Nothing to shoot at, safely exit

        GameObject Bullet = bulletpool.GetPObj();
        Bullet.transform.position = transform.position;
        Bullet.SetActive(true);

        Vector2 shootdirection = (enemiesinrange[0].transform.position - transform.position).normalized;
        Bullet.GetComponent<Bullet>().Shoot(data, shootdirection);
    }


    private void Enemygone(Enemies enemies)
    {
        enemiesinrange.Remove(enemies);
    }
}
