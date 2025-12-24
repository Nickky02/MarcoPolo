using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        // Face the camera, but lock the vertical axis so he doesn't lean backward
        transform.forward = Camera.main.transform.forward;
    }
}