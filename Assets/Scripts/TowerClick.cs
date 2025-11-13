using UnityEngine;
using UnityEngine.EventSystems;

public class TowerClick : MonoBehaviour
{
    public MeleeTower tower;

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (tower != null )
            tower.UpgradeTower();
    }
}
