using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUIManager : MonoBehaviour
{
    public static TowerUIManager instance;

    public GameObject panel;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;
    public Button upgradeButton;
    public Button sellButton;

    private Tower currentTower;

    private void Awake()
    {
        instance = this;
        panel.SetActive(false);
    }

    public void Show(Tower tower)
    {
        currentTower = tower;
        upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = $" Upgrade: {tower.upgradeCost} Gold";
        sellButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Sell: {tower.upgradeCost / 2} Gold";
        //panel.transform.position = Camera.main.WorldToScreenPoint(tower.transform.position + Vector3.up * 2f);
        panel.SetActive(true);
    }

    public void Hide()
    {
        currentTower = null;
        panel.SetActive(false);
    }

    public void OnUpgradeClick()
    {
        if (currentTower != null)
        {
            currentTower.Upgrade();
            Show(currentTower);
        }
        Hide();
    }


    public void OnSellClick()
    {
        if (currentTower != null)
        {
            GoldManager.instance.AddGold(currentTower.upgradeCost / 2);
            if (currentTower.buildSpotPrefab != null)
            {
                Instantiate(currentTower.buildSpotPrefab, currentTower.buildSpotOrigin.position, Quaternion.identity);
            }
            Destroy(currentTower.gameObject);
            Hide();
        }
    }
}
