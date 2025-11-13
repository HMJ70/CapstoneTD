using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradePanelController : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text dmgText;
    [SerializeField] private TMP_Text rangeText;
    [SerializeField] private TMP_Text costText;

    [Header("Button")]
    [SerializeField] private Button upgradeButton;

    private MeleeTower currentTower;

    private void Awake()
    {
        upgradeButton.onClick.AddListener(OnUpgradeClicked);
    }

    public void OpenPanel(MeleeTower tower)
    {
        currentTower = tower;
        gameObject.SetActive(true);
        UpdatePanel();
    }

    private void UpdatePanel()
    {
        if (currentTower == null) return;

        levelText.text = $"Level: {currentTower.runtimeData.level}/{currentTower.runtimeData.maxLevel}";
        dmgText.text = $"Damage: {currentTower.runtimeData.dmg:F1}";
        rangeText.text = $"Range: {currentTower.runtimeData.range:F1}";

        if (currentTower.runtimeData.level >= currentTower.runtimeData.maxLevel)
        {
            costText.text = "Max Level";
            upgradeButton.interactable = false;
        }
        else
        {
            costText.text = $"Upgrade Cost: {currentTower.runtimeData.upgradeCost}";
            upgradeButton.interactable = GManager.instance.loots >= currentTower.runtimeData.upgradeCost;
        }
    }

    private void OnUpgradeClicked()
    {
        if (currentTower == null) return;

        currentTower.UpgradeTower();
        UpdatePanel();
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
        currentTower = null;
    }
}
