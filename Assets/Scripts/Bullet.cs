using UnityEngine;

public class Bullet : MonoBehaviour
{
    private TowerDatas datas;
    private Vector3 shootdirections;
    private float bulletdurations;
    void Start()
    {
        transform.localScale = Vector3.one * datas.bulletsize;
    }
    void Update()
    {
        if (bulletdurations <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            bulletdurations -= Time.deltaTime;
            transform.position += new Vector3(shootdirections.x, shootdirections.y) * datas.bulletspeed * Time.deltaTime;
        }
    }

    public void Shoot(TowerDatas data, Vector3 shootdirection)
    {
        datas = data;
        shootdirections = shootdirection;
        bulletdurations = datas.bulletduration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            Enemies enemies = collision.GetComponent<Enemies>();
            enemies.TakeDMG(datas.dmg);
            gameObject.SetActive(false);
        }
    }
}
