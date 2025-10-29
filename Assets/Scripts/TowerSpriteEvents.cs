using UnityEngine;

public class TowerSpriteEvents : MonoBehaviour
{
    public MeleeTower towerParent;

    public void Fire()
    {
        if (towerParent != null)
            towerParent.Fire();
    }
}
