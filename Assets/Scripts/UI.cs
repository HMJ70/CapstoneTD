using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;
using System;

public class UI : MonoBehaviour
{
    public static UI instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private TMP_Text wave;
    [SerializeField] private TMP_Text HP;
    [SerializeField] private TMP_Text Coins;
    [SerializeField] private TMP_Text Warning;
    [SerializeField] private TMP_Text OBJectivetext;


    [Header("Panels & Prefabs")]
    [SerializeField] private GameObject Tpanel;
    [SerializeField] private GameObject TcardPrefab;
    [SerializeField] private Transform cardcontainer;
    [SerializeField] private GameObject pausemenu;
    [SerializeField] private GameObject gameover;
    [SerializeField] private GameObject missioncomplete;


    [Header("Buttons")]
    [SerializeField] private Button speed1;
    [SerializeField] private Button speed2;
    [SerializeField] private Button speed3;
    [SerializeField] private Button pause;
    [SerializeField] private Button nextlvlbutton;

    [Header("Visuals")]
    [SerializeField] private Color normalbuttoncolor = Color.white;
    [SerializeField] private Color selectedbutton = Color.blue;
    [SerializeField] private Color normaltextcolor = Color.black;
    [SerializeField] private Color selectedtext = Color.white;

    [Header("Gameplay")]
    [SerializeField] private TowerDatas[] towers;

    [SerializeField]
    private string[] tutorialScenes =
{
    "Tutorial",
    "Tutorial2",
    "Tutorial3"
};


    private bool inGameScene = false;
    private bool isgamepaused = false;
    private List<GameObject> activeCards = new List<GameObject>();
    private Platforms currplatform;
    private bool missioncompletedsoundplayed = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        EnemySpawner.OnWchanged += UpdateWave;
        GManager.OnHPChange += UpdateHP;
        GManager.OnCoinsChange += UpdateCoins;
        Platforms.OnPlatformsClicked += HPlatformClicked;
        TCard.ontowerselected += HTowerSelected;
        EnemySpawner.Onmissioncomplete += ShowMissionComplete;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        EnemySpawner.OnWchanged -= UpdateWave;
        GManager.OnHPChange -= UpdateHP;
        GManager.OnCoinsChange -= UpdateCoins;
        Platforms.OnPlatformsClicked -= HPlatformClicked;
        TCard.ontowerselected -= HTowerSelected;
        EnemySpawner.Onmissioncomplete -= ShowMissionComplete;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        speed1.onClick.AddListener(() =>
        {
            SetGameSpeed(0.5f);
            Audiomanage.instance.playspeedslow();
        });
        speed2.onClick.AddListener(() =>
        {
            SetGameSpeed(1f);
            Audiomanage.instance.playspeednormal();
        });
        speed3.onClick.AddListener(() =>
        {
            SetGameSpeed(2f);
            Audiomanage.instance.playspeedfast();
        });

        if (GManager.instance != null)
            HighlightSelectedSpeedButton(GManager.instance.Gamespeed);
    }

    private void Update()
    {
        if (!inGameScene) return;
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            TogglePause();
    }

    private void UpdateWave(int currwave)
    {
        if (wave == null) return;
        wave.text = $"Wave: {currwave + 1}";
    }

    private void UpdateHP(int currHP)
    {
        if (HP == null) return;
        HP.text = $"Lives: {currHP}";
        if (currHP <= 0)
            ShowGameOver();
    }

    private void UpdateCoins(int currcoins)
    {
        if (Coins == null) return;
        Coins.text = $"Coins: {currcoins}";
    }

    private void HPlatformClicked(Platforms platforms)
    {
        currplatform = platforms;
        ShowTPanel();
    }

    private void ShowTPanel()
    {
        if (Tpanel == null) return;
        Tpanel.SetActive(true);
        Platforms.Tpanelopen = true;
        GManager.instance.SetTimeScale(0f);
        PopulateTCards();
        Audiomanage.instance.playpanel();
    }

    public void HideTPanel()
    {
        if (Tpanel == null) return;
        Tpanel.SetActive(false);
        Platforms.Tpanelopen = false;
        GManager.instance.SetTimeScale(GManager.instance.Gamespeed);
    }

    private void PopulateTCards()
    {
        foreach (var card in activeCards)
            Destroy(card);

        activeCards.Clear();

        foreach (var data in towers)
        {
            GameObject cardGO = Instantiate(TcardPrefab, cardcontainer);
            TCard card = cardGO.GetComponent<TCard>();
            card.initialize(data);
            activeCards.Add(cardGO);
        }
    }

    private void HTowerSelected(TowerDatas towerDatas)
    {
        if (currplatform == null) return;

        if (currplatform.transform.childCount > 1)
        {
            HideTPanel();
            StartCoroutine(ShowWarning("Already Has A Tower!"));
            return;
        }

        if (GManager.instance.loots >= towerDatas.price)
        {
            Audiomanage.instance.playtowerplaced();
            GManager.instance.spendmoney(towerDatas.price);
            currplatform.PlaceTower(towerDatas);
        }
        else
        {
            StartCoroutine(ShowWarning("Not Enough Coins!"));
        }

        HideTPanel();
    }

    public IEnumerator ShowWarning(string message)
    {
        if (Warning == null) yield break;
        Warning.text = message;
        Audiomanage.instance.playwarning();
        Warning.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(5f);
        Warning.gameObject.SetActive(false);
    }

    private void SetGameSpeed(float timescale)
    {
        HighlightSelectedSpeedButton(timescale);
        GManager.instance.setgamespeed(timescale);
    }

    private void UpdateButtonVisual(Button button, bool isSelected)
    {
        if (button == null) return;
        button.image.color = isSelected ? selectedbutton : normalbuttoncolor;
        TMP_Text text = button.GetComponentInChildren<TMP_Text>();
        if (text != null)
            text.color = isSelected ? selectedtext : normaltextcolor;
    }

    private void HighlightSelectedSpeedButton(float selectedSpeed)
    {
        UpdateButtonVisual(speed1, selectedSpeed == 0.5f);
        UpdateButtonVisual(speed2, selectedSpeed == 1f);
        UpdateButtonVisual(speed3, selectedSpeed == 2f);
    }

    public void TogglePause()
    {
        if (Tpanel != null && Tpanel.activeSelf)
            return;

        if (isgamepaused)
        {
            if (pausemenu != null) pausemenu.SetActive(false);
            isgamepaused = false;
            GManager.instance.SetTimeScale(GManager.instance.Gamespeed);
            Audiomanage.instance.playresume();
        }
        else
        {
            if (pausemenu != null) pausemenu.SetActive(true);
            isgamepaused = true;
            GManager.instance.SetTimeScale(0f);
            Audiomanage.instance.playpause();
        }
    }

    public void RestartLVL()
    {
        lvlmanager.instance.loadlevel(lvlmanager.instance.currlvl);
    }

    public void Quit() => Application.Quit();

    public void GoToMainMenu()
    {
        GManager.instance.SetTimeScale(1f);
        SceneManager.LoadScene("MainMenu");
    }

    private void ShowGameOver()
    {
        GManager.instance.SetTimeScale(0f);
        if (gameover != null) gameover.SetActive(true);
        Audiomanage.instance.playgameover();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SetupSceneUI(scene));
    }

    private IEnumerator SetupSceneUI(Scene scene)
    {
        yield return null;

        Camera maincam = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null && maincam != null)
            canvas.worldCamera = maincam;

        HidePanels();
        missioncompletedsoundplayed = false;

        if (scene.name == "MainMenu")
        {
            inGameScene = false;
            HideUI();
        }
        else
        {
            // If it's a tutorial, hide all UI
            if (Array.Exists(tutorialScenes, t => t == scene.name))
            {
                inGameScene = false;
                HideUI();
            }
            else
            {
                // Real gameplay scene
                inGameScene = true;
                ShowUI();
                StartCoroutine(ShowObjective());
            }
        }
    }

    private IEnumerator ShowObjective()
    {
        if (OBJectivetext == null || lvlmanager.instance == null) yield break;
        OBJectivetext.text = $"Survive {lvlmanager.instance.currlvl.wavestowin} Waves!";
        OBJectivetext.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        OBJectivetext.gameObject.SetActive(false);
    }

    private void ShowMissionComplete()
    {
        if (!missioncompletedsoundplayed)
        {
            updatenextlvlbutton();
            if (missioncomplete == null) return;
            missioncomplete.SetActive(true);
            GManager.instance.SetTimeScale(0f);
            Audiomanage.instance.playmissioncomplete();
            missioncompletedsoundplayed = true;
        }
    }

    public void EnterEndlessMode()
    {
        if (missioncomplete != null)
            missioncomplete.SetActive(false);
        GManager.instance.SetTimeScale(GManager.instance.Gamespeed);
        EnemySpawner.Instance.EnableEndlessMode();
    }

    private void HideUI()
    {
        HidePanels();
        if (Warning != null) Warning.gameObject.SetActive(false);
        if (wave != null) wave.gameObject.SetActive(false);
        if (HP != null) HP.gameObject.SetActive(false);
        if (Coins != null) Coins.gameObject.SetActive(false);
        if (speed1 != null) speed1.gameObject.SetActive(false);
        if (speed2 != null) speed2.gameObject.SetActive(false);
        if (speed3 != null) speed3.gameObject.SetActive(false);
        if (pause != null) pause.gameObject.SetActive(false);
    }

    private void ShowUI()
    {
        if (wave != null) wave.gameObject.SetActive(true);
        if (HP != null) HP.gameObject.SetActive(true);
        if (Coins != null) Coins.gameObject.SetActive(true);
        if (speed1 != null) speed1.gameObject.SetActive(true);
        if (speed2 != null) speed2.gameObject.SetActive(true);
        if (speed3 != null) speed3.gameObject.SetActive(true);
        HighlightSelectedSpeedButton(GManager.instance.Gamespeed);
        if (pause != null) pause.gameObject.SetActive(true);
    }

    private void HidePanels()
    {
        if (pausemenu != null) pausemenu.SetActive(false);
        if (gameover != null) gameover.SetActive(false);
        if (missioncomplete != null) missioncomplete.SetActive(false);
    }

    public void nextlevel()
    {
        var levelmanager = lvlmanager.instance;
        int currindex = Array.IndexOf(levelmanager.alllevels, levelmanager.currlvl);
        int nextindex = currindex + 1;
        if (nextindex < levelmanager.alllevels.Length)
        {
            levelmanager.loadlevel(levelmanager.alllevels[nextindex]);
        }
    }

    private void updatenextlvlbutton()
    {
        var levelmanager = lvlmanager.instance;
        int currindex = Array.IndexOf(levelmanager.alllevels, levelmanager.currlvl);
        nextlvlbutton.interactable = currindex + 1 < levelmanager.alllevels.Length;
    }
}
