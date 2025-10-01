using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text wave;
    [SerializeField] private TMP_Text HP;

    private void OnEnable()
    {
        EnemySpawner.OnWchanged += UpdateWave;
        GManager.OnHPChange += UpdateHP;
    }
    private void OnDisable()
    {
        EnemySpawner.OnWchanged -= UpdateWave;
    }
    private void UpdateWave(int currwave)
    {
        wave.text = $"Wave: {currwave + 1}";
    }

    private void UpdateHP(int currHP)
    {
        HP.text = $"Lives: {currHP}";
    }
}
