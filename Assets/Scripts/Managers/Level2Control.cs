using UnityEngine;

public class HardLevelControl : MissionControl
{
    protected override void SetupMission()
    {
        // Harder start
        currentMoney = 100; // Less money
        currentHealth = 50; // Less health

        // Debug.Log("--- LEVEL 2: Challenging Mode Starting ---");
    }
}