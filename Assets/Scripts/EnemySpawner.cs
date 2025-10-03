using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static event Action<int> OnWchanged;

    [SerializeField] private WData[] waves;

    private int CountW = 0;
    private int currwaveindex = 0;
    private WData currwave => waves[currwaveindex];

    private float STime;
    private float SCounter;
    private int ERemoved;

    [SerializeField] private ObjPool EnemyPool1;
    [SerializeField] private ObjPool EnemyPool2;
    [SerializeField] private ObjPool EnemyPool3;

    private Dictionary<EType, ObjPool> poolDict;

    private float WDelay = 1f;
    private float WCooldown;
    private bool isbetweenW = false;


    private void Awake()
    {
        poolDict = new Dictionary<EType, ObjPool>()
        {
            { EType.enemy1, EnemyPool1},
            { EType.enemy2, EnemyPool2},
            { EType.enemy3, EnemyPool3},
        };
    }

    private void OnEnable()
    {
        Enemies.OnReachingBase += EReachingBase;
        Enemies.OnEnemyKilled += EKilled;
    }

    private void OnDisable()
    {
        Enemies.OnReachingBase -= EReachingBase;
        Enemies.OnEnemyKilled -= EKilled;
    }

    private void Start()
    {
        OnWchanged?.Invoke(CountW);
    }
    void Update()
    {
        if(isbetweenW)
        {
            WCooldown -= Time.deltaTime;
            if(WCooldown <= 0f )
            {
                currwaveindex = (currwaveindex + 1) % waves.Length;
                CountW++;
                OnWchanged?.Invoke(CountW);
                SCounter = 0;
                ERemoved = 0;
                STime = 0f;// can be set to zero =0f; for instant or currwave.Sinterval
                isbetweenW = false;
            }
        }
        else
        {
            STime -= Time.deltaTime;
            if (STime <= 0 && SCounter < currwave.EperWave)
            {
                STime = currwave.Sinterval;
                SpawnE();
                SCounter++;
            }
            else if (SCounter >= currwave.EperWave && ERemoved >= currwave.EperWave)
            {
                isbetweenW = true;
                WCooldown = WDelay;
            }
        }
    }

    private void SpawnE()
    {
        if(poolDict.TryGetValue(currwave.Etype, out var pool))
        {
            GameObject SpawnedObj = pool.GetPObj();
            SpawnedObj.transform.position = transform.position;
            float hpmultiplier = 1f + (CountW * 0.1f); //10%health increase bruh
            Enemies enemies = SpawnedObj.GetComponent<Enemies>();
            enemies.Initialize(hpmultiplier);
            SpawnedObj.SetActive(true);
        }
    }

    private void EReachingBase(EData data)
    {
        ERemoved++;
    }

    private void EKilled(Enemies enemies)
    {
        ERemoved++;
    }
}
