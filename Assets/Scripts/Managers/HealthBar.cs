using UnityEngine;
using UnityEngine.UI;
using TMPro; // THIS LIBRARY IS REQUIRED to use TextMeshPro!

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    public TextMeshProUGUI healthText; // New variable for the text box

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        // Update the Bar
        fillImage.fillAmount = currentHealth / maxHealth;

        // Update the Text

        // ToString("0"): Print the number as an integer
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString("0") + " / " + maxHealth.ToString("0");
        }
    }
}