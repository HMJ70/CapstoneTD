using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private WData[] waves;

    private int currwaveindex = 0;
    private WData currwave => waves[currwaveindex];

    private float STime;
    private float SCounter;
    private int ERemoved;

    [SerializeField] private ObjPool EnemyPool1;
    [SerializeField] private ObjPool EnemyPool2;
    [SerializeField] private ObjPool EnemyPool3;

    private Dictionary<EType, ObjPool> poolDict;

    private void Awake()
    {
        poolDict = new Dictionary<EType, ObjPool>()
        {
            { EType.enemy1, EnemyPool1},
            { EType.enemy2, EnemyPool2},
            { EType.enemy3, EnemyPool3},
        };
    }

    void Update()
    {
        STime -= Time.deltaTime;
        if (STime <= 0 && SCounter < currwave.EperWave)
        {
            STime = currwave.Sinterval;
            SpawnE();
            SCounter++;
        }
        else if(SCounter >= currwave.EperWave && ERemoved >= currwave.EperWave)
        {
            currwaveindex = (currwaveindex + 1) % waves.Length;
            SCounter = 0;
        }
    }

    private void SpawnE()
    {
        if(poolDict.TryGetValue(currwave.Etype, out var pool))
        {
            GameObject SpawnedObj = pool.GetPObj();
            SpawnedObj.transform.position = transform.position;
            SpawnedObj.SetActive(true);
        }
    }
}
