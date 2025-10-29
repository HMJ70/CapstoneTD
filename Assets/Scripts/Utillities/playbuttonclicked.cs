using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class playbuttonclicked : MonoBehaviour, IPointerEnterHandler
{
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() =>
        {
            Audiomanage.instance.playbuttonclicked();
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Audiomanage.instance.playbuttonhover();
    }
}
