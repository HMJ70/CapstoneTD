using UnityEngine;

[CreateAssetMenu(fileName = "TowerDatas", menuName = "Scriptable Objects/TowerDatas")]
public class TowerDatas : ScriptableObject
{
    [Header("General Stats")]
    public float range = 3f;
    public float attackdelay = 1f;
    public float bulletspeed = 5f;
    public float bulletduration = 1f;
    public float bulletsize = 1f;
    public float dmg = 1f;
    public int price = 50;

    [Header("Visuals & Prefabs")]
    public Sprite sprite;
    public GameObject prefab;

    [Header("Optional: Tack Shooter Settings")]
    public bool isTackShooter = false; 
    public int tackCount = 8;         
    public float tackSpread = 360f;    

}
