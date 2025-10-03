using System;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager instance { get; private set; }
    public static event Action<int> OnHPChange;
    public static event Action<int> OnCoinsChange;
    private int loot = 0;
    private int HP = 20;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void OnEnable()
    {
        Enemies.OnReachingBase += HEReachingBase;
        Enemies.OnEnemyKilled += HEkilled;
    }
    private void OnDisable()
    {
        Enemies.OnReachingBase -= HEReachingBase;
        Enemies.OnEnemyKilled -= HEkilled;
    }
    private void HEReachingBase(EData data)
    {
        HP = Mathf.Max (0, HP - data.dmg);
        OnHPChange?.Invoke(HP);
    }
    private void Start()
    {
        OnHPChange?.Invoke(HP);
        OnCoinsChange?.Invoke(loot);
    }
    private void HEkilled(Enemies enemies)
    {
        lootgain(Mathf.RoundToInt(enemies.Dataz.Eloot));
    }
    private void lootgain(int amount)
    {
        loot += amount;
        OnCoinsChange?.Invoke(loot);
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }
    
}
