using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private int wavepointIndex = 0; // This is the correct variable name

    private Enemy enemy;

    // NEW: Custom roadmap for the enemy to follow
    private Waypoints myPath;

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // THE SPAWNER WILL CALL THIS FUNCTION
    public void SetPath(Waypoints path)
    {
        myPath = path; // Learned my path
        target = myPath.points[0]; // Locked onto the first target
    }

    void Update()
    {
        // If my path is not assigned, wait (to prevent errors)
        if (myPath == null) return;

        // --- MOVEMENT CODES ---
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.GetCurrentSpeed() * Time.deltaTime, Space.World);

        // Distance Calculation (for towers)
        float distanceToCurrentPoint = Vector3.Distance(transform.position, target.position);

        // We use 'wavepointIndex' here, correct.
        enemy.DistanceToGoal = distanceToCurrentPoint + ((myPath.points.Length - wavepointIndex) * 1000);

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (wavepointIndex >= myPath.points.Length - 1)
        {
            EndPath();
            return;
        }


        wavepointIndex++;

        target = myPath.points[wavepointIndex];
    }

    void EndPath()
    {
        if (MissionControl.Instance != null)
        {
            // Get enemy-specific damage (like Draugr 5, Hrimthurs 10)
            int damage = enemy.GetDamageToBase();

            int remainingHealth = MissionControl.Instance.GetCurrentHealth();

            remainingHealth -= damage;

            if (remainingHealth >= 0)
                GameLogger.Write($"'{gameObject.name}' reached Valhalla. Remaining Health: {remainingHealth} Damage Taken (-{damage}).");
            else
                GameLogger.Write($"'{gameObject.name}' reached Valhalla. Remaining Health: {0} Damage Taken (-{damage}).");

            // Decrease health
            MissionControl.Instance.TakeDamage(damage);

        }

        // Destroy
        Destroy(gameObject);
    }
}