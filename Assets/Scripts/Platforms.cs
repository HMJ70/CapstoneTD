using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class Platforms : MonoBehaviour
{
    public static event Action<Platforms> OnPlatformsClicked;
    [SerializeField] private LayerMask platformlayermask;
    public static bool Tpanelopen { get; set; } = false;
    private void Update()
    {
        if (Tpanelopen || Time.timeScale == 0f)
        {
            return;
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D raycasthit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, platformlayermask);
            if(raycasthit.collider != null)
            {
                Platforms platforms = raycasthit.collider.GetComponent<Platforms>();
                if(platforms != null)
                {
                    OnPlatformsClicked?.Invoke(platforms);
                }
            }
        }
    }

    public void PlaceTower(TowerDatas data)
    {
        Instantiate(data.prefab,transform.position,Quaternion.identity,transform);
    }
}
