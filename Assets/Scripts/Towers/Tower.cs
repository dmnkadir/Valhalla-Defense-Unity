using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    // Common properties for all towers
    [SerializeField] protected float range;            // Tower range
    [SerializeField] protected float damage;           // Tower damage
    [SerializeField] protected float fireRate;         // Fire rate (number of shots per second)

    protected Transform currentTarget;

    protected float fireCountdown = 0f;
    protected bool canAttackFlying = true;

    public virtual int Cost { get { return 50; } }      //Tower price default is 50 


    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float shortestDistanceToGoal = Mathf.Infinity; // Shortest path remaining to the base (record)
        GameObject nearestEnemyToBase = null; // Candidate enemy nearest to the base

        foreach (GameObject enemyObj in enemies)
        {
            float distanceToTower = Vector3.Distance(transform.position, enemyObj.transform.position); // Distance between tower and enemy

            if (distanceToTower <= range) // If within range
            {
                // Access the enemy script so we can read the "DistanceToGoal" value
                Enemy enemyScript = enemyObj.GetComponent<Enemy>();

                if (enemyScript != null)
                {
                    if (!canAttackFlying && enemyScript.IsFlying)
                    {
                        continue;
                    }
                    if (enemyScript.DistanceToGoal < shortestDistanceToGoal)
                    {
                        shortestDistanceToGoal = enemyScript.DistanceToGoal;
                        nearestEnemyToBase = enemyObj;
                    }
                }
            }
        }


        if (nearestEnemyToBase != null) // If the nearest enemy is found
        {
            currentTarget = nearestEnemyToBase.transform;
        }
        else
        {
            currentTarget = null;
        }
    }
    protected virtual void Update()
    {
        // If I have no target (null), do nothing, wait.
        if (currentTarget == null)
            return;

        // Decrease the countdown timer (Time.deltaTime = elapsed time).
        fireCountdown -= Time.deltaTime;

        // Has the timer reached zero?
        if (fireCountdown <= 0f)
        {
            Shoot(); // SHOOT! (We'll make this function abstract, children will fill it)

            // Reset the timer. 
            // if fireRate is 1 -> wait 1 second.
            // if fireRate is 2 -> wait 0.5 seconds.
            fireCountdown = 1f / fireRate;
        }
    }

    protected abstract void Shoot();
    protected virtual void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}