using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float STime;
    private float SInterval = 1f;

    [SerializeField] private ObjPool Pool;

    void Update()
    {
        STime -= Time.deltaTime;
        if (STime <= 0)
        {
            STime = SInterval;
            SpawnE();
        }
    }

    private void SpawnE()
    {
        GameObject SpawnedObj = Pool.GetPObj();
        SpawnedObj.transform.position = transform.position;
        SpawnedObj.SetActive(true);
    }
}
