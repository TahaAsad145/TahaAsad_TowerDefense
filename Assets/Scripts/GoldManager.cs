using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public static GoldManager instance;
    public int currentGold = 0;
    public TextMeshProUGUI goldText;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateGoldUI();
    }

    public void AddGold(int coins)
    {
        currentGold += coins;
        UpdateGoldUI();
    }

    void UpdateGoldUI()
    {
        if(goldText != null)
        {
            goldText.text = "Gold: " + currentGold;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
