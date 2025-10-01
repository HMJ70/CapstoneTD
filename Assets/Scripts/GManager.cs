using System;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static event Action<int> OnHPChange;

    private int HP = 20;
    private void OnEnable()
    {
        Enemies.OnReachingBase += HEReachingBase;
    }
    private void OnDisable()
    {
        Enemies.OnReachingBase -= HEReachingBase;
    }
    private void HEReachingBase(EData data)
    {
        HP = Mathf.Max (0, HP - data.dmg);
        OnHPChange?.Invoke(HP);
    }
    private void Start()
    {
        OnHPChange?.Invoke(HP);
    }
}
