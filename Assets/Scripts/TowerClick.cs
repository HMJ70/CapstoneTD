using UnityEngine;

public class TowerClick : MonoBehaviour
{
    public MeleeTower tower;

    private void OnMouseDown()
    {
        if (tower != null )
            tower.UpgradeTower();
    }
}
