using UnityEngine;

public class Valkyrie : Enemy
{
    public override float BaseHealth { get { return 50f; } }
    public override bool IsFlying
    {
        get
        {
            return true; // We said, "Yes, I'm flying!"
        }
    }
    protected override void Start() // Function that runs when the enemy is created // OVERRIDE
    {
        speed = 3.75f;           // Flying speed (75) 
        armor = 0f;             // No armor (0)
        killReward = 15;        // Money earned when killed
        damageToBase = 5;       // Damage dealt upon reaching the base



        base.Start();

    }
}