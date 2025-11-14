using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class TCard : MonoBehaviour
{
    [SerializeField] private Image timage;
    [SerializeField] private TMP_Text pricetext;
    [SerializeField] private TMP_Text Dmg;
    [SerializeField] private TMP_Text range;
    [SerializeField] private TMP_Text atkspeed;

    private TowerDatas towerdatas;
    public static event Action<TowerDatas> ontowerselected;
    public void initialize(TowerDatas data)
    {
        towerdatas = data;
        timage.sprite = data.sprite;
        pricetext.text = "Price: " + data.price;
        Dmg.text = "DMG: " + data.dmg;
        range.text = "Range: " + data.range;
        atkspeed.text = "Speed: " + data.attackdelay;
    }

    public void placetower()
    {
        ontowerselected?.Invoke(towerdatas);
    }
}
