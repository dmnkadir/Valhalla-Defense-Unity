using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Transform target;

    [Header("Settings")]
    public float speed = 5.0f;      // our speed
    public float hitRadius = 0.5f; //  Hit when it gets this close to the enemy

    private float damage;
    private bool hasHit = false;   // Prevents calling hit twice for the same arrow

    public void Seek(Transform _target, float _damage)
    {
        target = _target;
        damage = _damage;
    }

    void Update()
    {
        // If the target is lost, destroy the arrow
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Direction and Distance Calculation
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // HIT RADIUS 
        // If the distance to the target is smaller than "hitRadius", consider it a HIT.
        // dir.magnitude (Remaining Path) <= distanceThisFrame (Path to Travel)
        if (dir.magnitude <= distanceThisFrame || dir.magnitude <= hitRadius)
        {
            HitTarget();
            return;
        }

        //  (Move in 3D space to close the height gap)
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        // (Rotate only on the X-Z plane, don't look at the ground)
        // To prevent the arrow from appearing to "crash into the ground" in mid-air:
        Vector3 lookDir = dir;
        lookDir.y = 0; // Rotate while ignoring height

        if (lookDir != Vector3.zero)
        {
            transform.LookAt(transform.position + lookDir);
            // Correction angle if the tip of the arrow faces right (try 90, -90, or 0 here based on your setup)
            transform.Rotate(90, -90, 0);
        }
    }

    void HitTarget()
    {
        if (hasHit) return; // If it already hit, don't hit again
        hasHit = true;

        Debug.Log(" ARROW HIT THE TARGET!");

        Enemy e = target.GetComponent<Enemy>();
        if (e != null)
        {
            e.TakeDamage(damage);
            Debug.Log(" Dealt " + damage + " damage to the enemy.");
        }

        Destroy(gameObject); // Destroy the arrow
    }
}