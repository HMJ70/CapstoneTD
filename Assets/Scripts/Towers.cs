using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Towers : MonoBehaviour
{
    [SerializeField] private TowerDatas data;

    private CircleCollider2D circleCollider;
    private List<Enemies> enemiesinrange;
    private ObjPool bulletpool;

    private float shootime;
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
        circleCollider.radius = data.range;
        enemiesinrange = new List<Enemies>();
        bulletpool = GetComponent<ObjPool>();
        shootime = data.attackdelay;
    }
    private void Update()
    {
        shootime -= Time.deltaTime;
        if(shootime <= 0)
        {
            shootime = data.attackdelay;
            Shoot();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,data.range);
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
            if(enemiesinrange.Contains(enemy))
            {
                enemiesinrange.Remove(enemy);
            }
        }
    }

    private void Shoot()
    {
        enemiesinrange.RemoveAll(enemies => enemies == null || !enemies.gameObject.activeInHierarchy);

        if(enemiesinrange.Count > 0)
        {
            GameObject Bullet = bulletpool.GetPObj();
            Bullet.transform.position = transform.position;
            Bullet.SetActive(true);
            Vector2 shootdirection = (enemiesinrange[0].transform.position - transform.position).normalized;
            Bullet.GetComponent<Bullet>().Shoot(data, shootdirection);
        }
    }

    private void Enemygone(Enemies enemies)
    {
        enemiesinrange.Remove(enemies);
    }

}
