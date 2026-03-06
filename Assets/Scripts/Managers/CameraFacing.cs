using UnityEngine;

public class CameraFacing : MonoBehaviour
{
    void LateUpdate() // will always face the camera
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}