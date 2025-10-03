using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text wave;
    [SerializeField] private TMP_Text HP;
    [SerializeField] private TMP_Text Coins;
    [SerializeField] private GameObject Tpanel;
    private void OnEnable()
    {
        EnemySpawner.OnWchanged += UpdateWave;
        GManager.OnHPChange += UpdateHP;
        GManager.OnCoinsChange += UpdateCoins;
        Platforms.OnPlatformsClicked += HPlatformClicked;
    }
    private void OnDisable()
    {
        EnemySpawner.OnWchanged -= UpdateWave;
        GManager.OnHPChange -= UpdateHP;
        GManager.OnCoinsChange -= UpdateCoins;
        Platforms.OnPlatformsClicked -= HPlatformClicked;
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
        ShowTPanel();
    }
    private void ShowTPanel()
    {
        Tpanel.SetActive(true);
        GManager.instance.SetTimeScale(0f);
    }

    public void HideTPanel()
    {
        Tpanel.SetActive(false);
        GManager.instance.SetTimeScale(1f);
    }
}
