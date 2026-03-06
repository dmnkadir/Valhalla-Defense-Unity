using UnityEngine;

using TMPro; // This library is required to use TextMeshPro

public class GameUI : MonoBehaviour
{
    [Header("Screen UI Elements")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI healthText;

    void Update()
    {
        // Pulling and printing real-time data from the MissionControl script
        if (MissionControl.Instance != null)
        {
            moneyText.text = " " + MissionControl.Instance.GetCurrentMoney();
            healthText.text = " " + MissionControl.Instance.GetCurrentHealth();
        }
    }
}