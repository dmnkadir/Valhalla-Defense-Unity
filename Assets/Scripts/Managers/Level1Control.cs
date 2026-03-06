using UnityEngine;

public class Level1Control : MissionControl
{

    protected override void SetupMission() // Customize initial settings
    {
        // Settings requested in the documentation:
        currentMoney = 200;
        currentHealth = 100;
        SetGameSpeed(1f);

        GameLogger.InitLog("Level 1 - Defense of Valhalla", currentHealth, currentMoney);

        // Log the first message
        GameLogger.Write("Level 1 parameters loaded. Waiting for enemies...");
        // Debug.Log("--- LEVEL 1: Easy Mode Started ---");
    }
}