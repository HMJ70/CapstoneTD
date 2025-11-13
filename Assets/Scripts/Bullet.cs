using UnityEngine;

public class Bullet : MonoBehaviour
{
    private TowerRuntimeData runtimeData;   // use runtime data
    private Vector3 shootDirection;
    private float bulletDuration;

    private void Update()
    {
        if (bulletDuration <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            bulletDuration -= Time.deltaTime;
            transform.position += shootDirection * runtimeData.bulletSpeed * Time.deltaTime;
        }
    }

    // Change argument type to TowerRuntimeData
    public void Shoot(TowerRuntimeData data, Vector3 direction)
    {
        runtimeData = data;
        shootDirection = direction.normalized;
        bulletDuration = runtimeData.bulletDuration;
        transform.localScale = Vector3.one * runtimeData.bulletSize;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemies enemy = collision.GetComponent<Enemies>();
            if (enemy != null)
                enemy.TakeDMG(runtimeData.dmg);

            gameObject.SetActive(false);
        }
    }
}
