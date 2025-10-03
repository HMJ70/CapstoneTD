using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text wave;
    [SerializeField] private TMP_Text HP;
    [SerializeField] private TMP_Text Coins;
    [SerializeField] private GameObject Tpanel;
    [SerializeField] private GameObject TcardPrefab;
    [SerializeField] private Transform cardcontainer;
    [SerializeField] private TowerDatas[] towers;

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
        GManager.instance.SetTimeScale(0f);
        PTcards();
    }
    public void HideTPanel()
    {
        Tpanel.SetActive(false);
        GManager.instance.SetTimeScale(1f);
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
        currplatform.PlaceTower(towerDatas);
        HideTPanel();
    }
}
