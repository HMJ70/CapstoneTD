[System.Serializable]
public class TowerRuntimeData
{
    public int level = 1;
    public int maxLevel = 3;         // max upgrade
    public int upgradeCost = 50;     // cost per upgrade
    public float range;
    public float attackDelay;
    public float dmg;
    public float bulletSpeed;
    public float bulletSize;
    public float bulletDuration;

    public TowerRuntimeData(TowerDatas data)
    {
        range = data.range;
        attackDelay = data.attackdelay;
        dmg = data.dmg;
        bulletSpeed = data.bulletspeed;
        bulletSize = data.bulletsize;
        bulletDuration = data.bulletduration;
    }

    public void Upgrade()
    {
        if (level >= maxLevel) return;

        level++;
        range *= 1.2f;          // example increase
        dmg *= 1.5f;
        attackDelay *= 0.9f;
        bulletSpeed *= 1.1f;
        bulletSize *= 1.1f;

    }
}
