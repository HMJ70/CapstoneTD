using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text wave;
    [SerializeField] private TMP_Text HP;
    [SerializeField] private TMP_Text Coins;
    private void OnEnable()
    {
        EnemySpawner.OnWchanged += UpdateWave;
        GManager.OnHPChange += UpdateHP;
        GManager.OnCoinsChange += UpdateCoins;
    }
    private void OnDisable()
    {
        EnemySpawner.OnWchanged -= UpdateWave;
        GManager.OnHPChange -= UpdateHP;
        GManager.OnCoinsChange -= UpdateCoins;
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
}
