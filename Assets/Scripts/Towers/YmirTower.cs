using UnityEngine;
using System.Collections; // For Coroutine (Timing) 

public class YmirTower : Tower
{
    public override int Cost { get { return 70; } }

    [Header("Visual Settings")]
    public Sprite idleSprite;   // 1st Image (Closed mouth/Idle)
    public Sprite attackSprite; // 2nd Image (Blowing)

    private SpriteRenderer sr;

    public AudioClip windSound;// wind sound

    protected override void Start()
    {
        range = 3.5f;       // Range
        damage = 15f;       // Damage
        fireRate = 1.0f;    // Fire Rate

        sr = GetComponent<SpriteRenderer>();

        // Load idle state when the game starts
        if (idleSprite != null) sr.sprite = idleSprite;

        base.Start();
    }

    // CONTINUOUS ROTATION 
    protected override void Update()
    {
        base.Update(); // Run father's target finding feature

        // Rotate to Target Process
        RotateToTarget();
    }

    void RotateToTarget()
    {
        if (currentTarget == null) return;

        // Is the enemy on my right or left?
        // Math: Enemy X - Tower X
        float direction = currentTarget.position.x - transform.position.x;


        if (direction > 0)
        {
            // Enemy on RIGHT -> Stay normal (Don't flip)
            sr.flipX = false;
        }
        else
        {
            // Enemy on LEFT -> Mirror (Flip)
            sr.flipX = true;
        }
    }

    protected override void Shoot()
    {
        if (currentTarget != null)
        {
            if (windSound != null)
                GetComponent<AudioSource>().PlayOneShot(windSound);

            // Start animation
            StartCoroutine(PlayAttackAnimation());

            Enemy enemyScript = currentTarget.GetComponent<Enemy>();

            if (enemyScript != null)
            {

                // Get values before hitting
                float currentHealth = enemyScript.GetCurrentHealth();
                float enemyArmor = enemyScript.GetArmor();

                // Calculate net damage
                float netDamage = enemyScript.CalculateNetDamage(damage);

                // Predict remaining health
                float remainingHealth = currentHealth - netDamage;
                if (remainingHealth < 0) remainingHealth = 0;

                // WRITE LOG (Log first, then process!)
                GameLogger.Write($"Tower '{gameObject.name}' froze target '{enemyScript.name}' with its breath.");
                GameLogger.Write($"   -> Armor: {enemyArmor} | Health: {currentHealth:0.0} -> {remainingHealth:0.0} | (Net Damage: {netDamage:0.0} + Slow)");

                // Deal true damage (Odin and Ymir specialty)
                enemyScript.TakeTrueDamage(damage); // Health will decrease now
                // Apply slow
                enemyScript.Slow(0.5f, 3f);         // Slow will be applied now
            }
        }
    }

    // Attack Animation (Switching Images)
    IEnumerator PlayAttackAnimation()
    {
        // Switch to blowing image
        if (attackSprite != null) sr.sprite = attackSprite;

        // Wait half a second (so blowing effect is visible)
        yield return new WaitForSeconds(0.5f);

        // Switch back to normal idle
        if (idleSprite != null) sr.sprite = idleSprite;
    }
}