using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class TCard : MonoBehaviour
{
    [SerializeField] private Image timage;
    [SerializeField] private TMP_Text pricetext;

    private TowerDatas towerdatas;
    public static event Action<TowerDatas> ontowerselected;
    public void initialize(TowerDatas data)
    {
        towerdatas = data;
        timage.sprite = data.sprite;
        pricetext.text = data.price.ToString();
    }

    public void placetower()
    {
        ontowerselected?.Invoke(towerdatas);
    }
}
