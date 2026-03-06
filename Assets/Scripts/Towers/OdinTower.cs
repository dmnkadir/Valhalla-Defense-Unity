using UnityEngine;

public class OdinTower : Tower // Inherits from Tower class 
{
    public override int Cost { get { return 75; } }

    [Header("Odin Special Settings")]
    public float explosionRadius = 0.75f;
    public GameObject runeEffectPrefab; //Effect that will appear in the hit area

    public AudioClip thunderSound; //thunder sound


    // Overriding father's Start function
    protected override void Start()
    {
        // We determine values specific to this tower inside the code
        range = 2.75f;       // Range
        damage = 20f;        // Damage
        fireRate = 0.33f;   // Fire Rate
        canAttackFlying = false;
        // Run (Tower) Start function as well so the target finding system begins
        base.Start();
    }

    // Overriding father's Shoot function
    protected override void Shoot()
    {
        if (currentTarget == null) return;

        // Visual and Sound Effects
        if (runeEffectPrefab != null)
            Instantiate(runeEffectPrefab, currentTarget.position, Quaternion.Euler(90, 0, 0));

        if (thunderSound != null)
            GetComponent<AudioSource>().PlayOneShot(thunderSound);

        // Log Title
        GameLogger.Write($"Tower '{gameObject.name}' struck lightning. (Center: {currentTarget.name})");

        // Area Damage (AOE) Detection
        Collider[] hitColliders = Physics.OverlapSphere(currentTarget.position, explosionRadius);

        foreach (Collider hit in hitColliders)
        {
            Enemy enemyScript = hit.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                if (enemyScript.IsFlying) continue;


                // Get data FIRST (We haven't hit yet!)
                float currentHealth = enemyScript.GetCurrentHealth(); // Enemy's current health
                float enemyArmor = enemyScript.GetArmor();
                float netDamage = enemyScript.CalculateNetDamage(damage); // Damage after armor is deducted

                // Calculate estimated remaining health (for Log)
                float remainingHealth = currentHealth - netDamage;
                if (remainingHealth < 0) remainingHealth = 0; // Don't show negative value in log

                // WRITE LOG 
                GameLogger.Write($"   -> Target: '{enemyScript.name}' | Armor: {enemyArmor} | Current Health: {currentHealth:0.0} -> Remaining: {remainingHealth:0.0} | (Net Damage: {netDamage:0.0})");

                // Deal true damage (Odin and Ymir specialty)
                enemyScript.TakeTrueDamage(damage);


            }
        }
    }

    // Gizmos (Visual Aid)
    void OnDrawGizmos()
    {
        // Red: Tower Attack Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);

        // YELLOW: Explosion Area 
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}