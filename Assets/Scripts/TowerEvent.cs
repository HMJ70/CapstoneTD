using UnityEngine;

public class TowerEvent : MonoBehaviour
{
    public Towers towerParent; 

    public void FireProjectile()
    {
        if (towerParent != null)
            towerParent.FireProjectile();
    }
}
