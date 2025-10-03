using UnityEngine;

[CreateAssetMenu(fileName = "TowerDatas", menuName = "Scriptable Objects/TowerDatas")]
public class TowerDatas : ScriptableObject
{
    public float range;
    public float attackdelay;
    public float bulletspeed;
    public float bulletduration;
    public float bulletsize;
    public float dmg;
}
