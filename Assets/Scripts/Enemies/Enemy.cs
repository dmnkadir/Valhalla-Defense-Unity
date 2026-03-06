using UnityEngine;
using System.Collections; // Required for IEnumerator
public abstract class Enemy : MonoBehaviour
{
    // Common properties for all enemies 
    protected float maxHealth;         // e.g., 50, 75
    protected float currentHealth;
    [SerializeField] protected float speed;
    [SerializeField] protected float currentSpeed;     // e.g., 50, 75
    protected float armor;             // True/False
    protected int killReward;          // e.g., 10, 20 money
    protected int damageToBase;        // Damage dealt upon reaching the base 

    public float DistanceToGoal = 10000f;

    public virtual float BaseHealth { get { return 50f; } }


    [Header("UI")]
    public HealthBar healthBar; // Health bar script
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public float GetMaxHealth()
    {
        if (maxHealth == 0) return BaseHealth;
        return maxHealth;
    }
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public int GetDamageToBase()
    {
        return damageToBase;
    }
    public virtual bool IsFlying // Is it a flying enemy?
    {
        get
        {
            return false;
        }   // Default as ground-walking enemy
    }
    public float GetArmor()
    {
        return armor;
    }

    public void Slow(float slowPercentage, float duration) // Function call that applies the slowing effect
    {
        StartCoroutine(ApplySlow(slowPercentage, duration));
    }

    IEnumerator ApplySlow(float pct, float time)
    {
        // LOWER THE SPEED
        currentSpeed = speed * (1f - pct);

        // Find the enemy's sprite component
        SpriteRenderer enemySR = GetComponent<SpriteRenderer>();

        // Keep the original color in memory (Maybe the enemy is already red, etc.)
        Color originalColor = Color.white;
        if (enemySR != null)
        {
            originalColor = enemySR.color;
            enemySR.color = Color.cyan; // Set to ICE COLOR (Light Blue)
        }

        Debug.Log(gameObject.name + " FROZE!");


        yield return new WaitForSeconds(time);

        // RETURN TO FORMER STATE
        currentSpeed = speed; // Correct the speed

        // --- COLOR CORRECTION ---
        if (enemySR != null)
        {
            enemySR.color = originalColor; // Revert color to original
        }
        // ---------------------

        Debug.Log("Ice thawed.");
    }

    protected virtual void Start() // Function that runs when the enemy is created
    {
        maxHealth = BaseHealth;
        currentHealth = maxHealth;
        currentSpeed = speed;
    }

    public float CalculateNetDamage(float incomingDamage) // Function returning net damage for logs
    {
        // Formula: Tower Damage * (1 - (Armor / (Armor + 100)))
        float damageReduction = armor / (armor + 100f);
        float netDamage = incomingDamage * (1f - damageReduction);

        return netDamage;
    }

    // Common function to run when enemy takes damage
    public virtual void TakeDamage(float incomingDamage) // For towers dealing damage based on armor (freya)
    {
        // Apply armor formula
        float netDamage = CalculateNetDamage(incomingDamage);

        // Hand over the process to the common function
        ApplyFinalDamage(netDamage);
    }
    public void TakeTrueDamage(float damage) // For towers that do not deal damage based on armor (Ymir, Odin)
    {
        // Hand over directly to the common function (No calculation)
        ApplyFinalDamage(damage);
    }
    private void ApplyFinalDamage(float finalDamage) // Function applying the damage
    {
        currentHealth -= finalDamage;

        // UI update (HealthBar)
        if (healthBar != null)
            healthBar.UpdateHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }



    // What happens when the enemy dies?
    protected virtual void Die()
    {
        if (MissionControl.Instance != null)
        {
            MissionControl.Instance.AddMoney(killReward);
            // logging operations
            int currentMoney = MissionControl.Instance.GetCurrentMoney();
            Debug.Log("Enemy died! {killReward} Gold earned.");
            GameLogger.Write($"'{gameObject.name}' died. Reward +{killReward}. Total Money: {currentMoney}.");
        }
        Destroy(gameObject); // Delete enemy from the scene
    }
}