using UnityEngine;

public class Hrimthurs : Enemy
{
    public override float BaseHealth { get { return 75f; } }
    protected override void Start() // Function that runs when the enemy is created // OVERRIDE
    {
        speed = 1.25f;            // Standard speed (50) 
        armor = 100f;             // No armor (0)
        killReward = 20;        // Money earned when killed
        damageToBase = 10;       // Damage dealt upon reaching the base

        base.Start();


    }
}