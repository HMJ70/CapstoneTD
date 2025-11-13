using UnityEngine;

public class TowerClickz : MonoBehaviour
{
    public Towers towerz; // assign the parent tower

    private void OnMouseDown()
    {
        if (towerz != null)
            towerz.UpgradeTower();
    }
}
