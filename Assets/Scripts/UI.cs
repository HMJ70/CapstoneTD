using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text wave;
    [SerializeField] private TMP_Text HP;
    [SerializeField] private TMP_Text Coins;
    [SerializeField] private GameObject Tpanel;
    [SerializeField] private GameObject TcardPrefab;
    [SerializeField] private Transform cardcontainer;
    [SerializeField] private TowerDatas[] towers;
    [SerializeField] private TMP_Text Warning;
    [SerializeField] private Button speed1;
    [SerializeField] private Button speed2;
    [SerializeField] private Button speed3;
    [SerializeField] private Color normalbuttoncolor = Color.white;
    [SerializeField] private Color selectedbutton = Color.blue;
    [SerializeField] private Color normaltextcolor = Color.black;
    [SerializeField] private Color selectedtext = Color.white;
    [SerializeField] private GameObject pausemenu;
    [SerializeField] private GameObject gameover;
    [SerializeField] private TMP_Text OBJectivetext;
    [SerializeField] private GameObject missioncomplete;

    private bool isgamepaused = false;
    private List<GameObject> activeCards = new List<GameObject>();
    private Platforms currplatform;
    private void OnEnable()
    {
        EnemySpawner.OnWchanged += UpdateWave;
        GManager.OnHPChange += UpdateHP;
        GManager.OnCoinsChange += UpdateCoins;
        Platforms.OnPlatformsClicked += HPlatformClicked;
        TCard.ontowerselected += HTowerSelected;
        SceneManager.sceneLoaded += OnSceneLoaded;
        EnemySpawner.Onmissioncomplete += showmissioncomplete;
    }
    private void OnDisable()
    {
        EnemySpawner.OnWchanged -= UpdateWave;
        GManager.OnHPChange -= UpdateHP;
        GManager.OnCoinsChange -= UpdateCoins;
        Platforms.OnPlatformsClicked -= HPlatformClicked;
        TCard.ontowerselected -= HTowerSelected;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EnemySpawner.Onmissioncomplete += showmissioncomplete;
    }
    private void Start()
    {
        speed1.onClick.AddListener(() => SetGameSpeed(0.5f));
        speed2.onClick.AddListener(() => SetGameSpeed(1f));
        speed3.onClick.AddListener(() => SetGameSpeed(2f));
        highlightselectedspeedbutton(GManager.instance.Gamespeed);
    }

    private void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }
    private void UpdateWave(int currwave)
    {
        wave.text = $"Wave: {currwave + 1}";
    }

    private void UpdateHP(int currHP)
    {
        HP.text = $"Lives: {currHP}";
        if(currHP <= 0)
        {
            ShowGameOver();
        }
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
        if(currplatform.transform.childCount > 1)
        {
            HideTPanel();
            StartCoroutine(ShowWarning("Already Has A Tower!"));
            return;
        }
        if(GManager.instance.loots >= towerDatas.price)
        {
            GManager.instance.spendmoney(towerDatas.price);
            currplatform.PlaceTower(towerDatas);
        }
        else
        {
            StartCoroutine(ShowWarning("Not Enough Coins!"));
        }
        HideTPanel();
    }

    private IEnumerator ShowWarning(string message)
    {
        Warning.text = message;
        Warning.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        Warning.gameObject.SetActive(false);
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

    public void TogglePause()
    {
        if(Tpanel.activeSelf)
        {
            return;
        }

        if (isgamepaused)
        {
            pausemenu.SetActive(false);
            isgamepaused = false;
            GManager.instance.SetTimeScale(GManager.instance.Gamespeed);
        }
        else
        {
            pausemenu.SetActive(true);
            isgamepaused = true;
            GManager.instance.SetTimeScale(0f);
        }
    }

    public void RestartLVL()
    {
        lvlmanager.instance.loadlevel(lvlmanager.instance.currlvl);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void gotomainmenu()
    {
        GManager.instance.SetTimeScale(1f);
        SceneManager.LoadScene("MainMenu");
    }

    private void ShowGameOver()
    {
        GManager.instance.SetTimeScale(0f);
        gameover.SetActive(true);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(showOBJective());
    }

    private IEnumerator showOBJective()
    {
        OBJectivetext.text = $"Survive {lvlmanager.instance.currlvl.wavestowin} Waves!";
        OBJectivetext.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        OBJectivetext.gameObject.SetActive(false);
    }

    private void showmissioncomplete()
    {
        missioncomplete.SetActive(true);
        GManager.instance.SetTimeScale(0f);
    }

    public void EnterEndlessmode()
    {
        missioncomplete.SetActive(false);
        GManager.instance.SetTimeScale(GManager.instance.Gamespeed);
        EnemySpawner.Instance.EnableEndlessMode();
    }
}
