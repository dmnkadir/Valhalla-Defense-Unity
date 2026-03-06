using UnityEngine;

public class FreyaTower : Tower
{
    public override int Cost { get { return 50; } }

    [Header("Visual Settings")]
    public Sprite rightSprite;
    public Sprite leftSprite;
    public GameObject arrowPrefab; // Arrow Prefab to be fired
    public Transform firePoint;    // Point where the arrow will exit (Muzzle tip)

    private SpriteRenderer sr;

    public AudioClip shootSound; // bow sound

    protected override void Start()
    {
        range = 2.75f;
        damage = 10f;
        fireRate = 1f;
        sr = GetComponent<SpriteRenderer>();
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        RotateToTarget();
    }

    void RotateToTarget()
    {
        if (currentTarget == null) return;
        float direction = currentTarget.position.x - transform.position.x;

        if (direction > 0)
        {
            if (sr.sprite != rightSprite) sr.sprite = rightSprite;
        }
        else
        {
            if (sr.sprite != leftSprite) sr.sprite = leftSprite;
        }
    }

    protected override void Shoot()
    {
        if (currentTarget != null)
        {
            if (shootSound != null)
                GetComponent<AudioSource>().PlayOneShot(shootSound);

            Enemy targetEnemy = currentTarget.GetComponent<Enemy>();

            if (targetEnemy != null)
            {

                // Fetch current data
                float currentHealth = targetEnemy.GetCurrentHealth();
                float enemyArmor = targetEnemy.GetArmor();

                // Calculate damage (But don't hit yet!)
                float netDamage = targetEnemy.CalculateNetDamage(damage);

                // Calculate estimated remaining health
                float remainingHealth = currentHealth - netDamage;
                if (remainingHealth < 0) remainingHealth = 0; // So negative values don't appear in the log

                // WRITE LOG 
                GameLogger.Write($"Tower '{gameObject.name}' fired its arrow. Target: {targetEnemy.name}");
                GameLogger.Write($"   -> Armor: {enemyArmor} | Health: {currentHealth:0.0} -> {remainingHealth:0.0} | (Net Damage: {netDamage:0.0})");

            }

            Vector3 spawnPos = transform.position + new Vector3(0, 3.4f, 0);

            // Create and fire the arrow (Damage process will occur in the Arrow script, but log is given here)
            GameObject bulletGO = Instantiate(arrowPrefab, spawnPos, Quaternion.identity);
            Arrow arrow = bulletGO.GetComponent<Arrow>();

            if (arrow != null)
            {
                arrow.Seek(currentTarget, damage);
            }
        }
    }
}