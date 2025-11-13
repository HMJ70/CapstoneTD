[System.Serializable]
public class TowerRuntimeData
{
    public int level = 1;
    public int maxLevel = 5;
    public int upgradeCost = 50;

    public float upgradeCostMultiplier = 2.0f; // increase factor per level
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
        range *= 1.05f;
        dmg *= 1.05f;
        attackDelay *= 0.99f;
        bulletSpeed *= 1.01f;
        bulletSize *= 1.01f;
    }
}
