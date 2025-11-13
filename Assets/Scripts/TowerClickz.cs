using UnityEngine;
using UnityEngine.EventSystems;

public class TowerClickz : MonoBehaviour
{
    public Towers towerz; // assign the parent tower

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (towerz != null)
            towerz.UpgradeTower();
    }
}
