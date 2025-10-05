using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text wave;
    [SerializeField] private TMP_Text HP;
    [SerializeField] private TMP_Text Coins;
    [SerializeField] private GameObject Tpanel;
    [SerializeField] private GameObject TcardPrefab;
    [SerializeField] private Transform cardcontainer;
    [SerializeField] private TowerDatas[] towers;
    [SerializeField] private GameObject notenoughcoins;
    [SerializeField] private Button speed1;
    [SerializeField] private Button speed2;
    [SerializeField] private Button speed3;
    [SerializeField] private Color normalbuttoncolor = Color.white;
    [SerializeField] private Color selectedbutton = Color.blue;
    [SerializeField] private Color normaltextcolor = Color.black;
    [SerializeField] private Color selectedtext = Color.white;


    private List<GameObject> activeCards = new List<GameObject>();
    private Platforms currplatform;
    private void OnEnable()
    {
        EnemySpawner.OnWchanged += UpdateWave;
        GManager.OnHPChange += UpdateHP;
        GManager.OnCoinsChange += UpdateCoins;
        Platforms.OnPlatformsClicked += HPlatformClicked;
        TCard.ontowerselected += HTowerSelected;
    }
    private void OnDisable()
    {
        EnemySpawner.OnWchanged -= UpdateWave;
        GManager.OnHPChange -= UpdateHP;
        GManager.OnCoinsChange -= UpdateCoins;
        Platforms.OnPlatformsClicked -= HPlatformClicked;
        TCard.ontowerselected -= HTowerSelected;
    }
    private void Start()
    {
        speed1.onClick.AddListener(() => SetGameSpeed(0.5f));
        speed2.onClick.AddListener(() => SetGameSpeed(1f));
        speed3.onClick.AddListener(() => SetGameSpeed(2f));
        highlightselectedspeedbutton(GManager.instance.Gamespeed);
    }
    private void UpdateWave(int currwave)
    {
        wave.text = $"Wave: {currwave + 1}";
    }

    private void UpdateHP(int currHP)
    {
        HP.text = $"Lives: {currHP}";
    }

    private void UpdateCoins(int currcoins)
    {
        Coins.text = $"Coins: {currcoins}";
    }

    private void HPlatformClicked(Platforms platforms)
    {
        currplatform = platforms;
        ShowTPanel();
    }
    private void ShowTPanel()
    {
        Tpanel.SetActive(true);
        Platforms.Tpanelopen = true;
        GManager.instance.SetTimeScale(0f);
        PTcards();
    }
    public void HideTPanel()
    {
        Tpanel.SetActive(false);
        Platforms.Tpanelopen = false;
        GManager.instance.SetTimeScale(GManager.instance.Gamespeed);
    }

    private void PTcards()
    {
        foreach(var card in activeCards)
        {
            Destroy(card);
        }
        activeCards.Clear();
        foreach(var data in towers)
        {
            GameObject cardgameobject = Instantiate(TcardPrefab, cardcontainer);
            TCard card = cardgameobject.GetComponent<TCard>();
            card.initialize(data);
            activeCards.Add(cardgameobject);
        }
    }

    private void HTowerSelected(TowerDatas towerDatas)
    {
        if(GManager.instance.loots >= towerDatas.price)
        {
            GManager.instance.spendmoney(towerDatas.price);
            currplatform.PlaceTower(towerDatas);
        }
        else
        {
            StartCoroutine(ShowNoCoinsMessage());
        }
        HideTPanel();
    }

    private IEnumerator ShowNoCoinsMessage()
    {
        notenoughcoins.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        notenoughcoins.SetActive(false);
    }

    private void SetGameSpeed(float timescale)
    {
        highlightselectedspeedbutton(timescale);
        GManager.instance.setgamespeed(timescale);
    }

    private void updatebuttonvisual(Button button, bool isselected)
    {
        button.image.color = isselected ? selectedbutton : normalbuttoncolor;
        TMP_Text text = button.GetComponentInChildren<TMP_Text>();
        if(text != null)
        {
            text.color = isselected ? selectedtext : normaltextcolor;
        }
    }

    private void highlightselectedspeedbutton(float selectedspeed)
    {
        updatebuttonvisual(speed1, selectedspeed == 0.5f);
        updatebuttonvisual(speed2, selectedspeed == 1f);
        updatebuttonvisual(speed3, selectedspeed == 2f);
    }

}
