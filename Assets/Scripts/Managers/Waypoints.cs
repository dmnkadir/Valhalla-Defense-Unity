using UnityEngine;

public class Waypoints : MonoBehaviour
{

    public Transform[] points; // Array to hold all waypoint points

    void Awake() // Runs before the game starts
    {
        points = new Transform[transform.childCount];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }
}