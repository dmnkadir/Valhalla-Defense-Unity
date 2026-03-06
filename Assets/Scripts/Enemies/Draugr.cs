using UnityEngine;

public class Draugr : Enemy
{
    public override float BaseHealth { get { return 50f; } }
    protected override void Start() // Function that runs when the enemy is created // OVERRIDE
    {
        speed = 2.5f;            // Standard speed (50) 
        armor = 0f;             // No armor (0)
        killReward = 10;        // Money earned when killed
        damageToBase = 5;       // Damage dealt upon reaching the base

        base.Start();

    }
}