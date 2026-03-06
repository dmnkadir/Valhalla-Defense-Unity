using UnityEngine;

public class EffectDestroyer : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1f); // Destroy itself after 1 second
    }
}