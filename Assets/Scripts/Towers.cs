using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Towers : MonoBehaviour
{
    [SerializeField] private TowerDatas data;

    private CircleCollider2D circleCollider;
    private List<Enemies> enemiesinrange;
    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = data.range;
        enemiesinrange = new List<Enemies>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,data.range);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemies enemies = GetComponent<Enemies>();
            enemiesinrange.Add(enemies);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemies enemies = GetComponent<Enemies>();
            if(enemiesinrange.Contains(enemies))
            {
                enemiesinrange.Remove(enemies);
            }
        }
    }
}
