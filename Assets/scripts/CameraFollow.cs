using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The object we want to follow (The Player)
    public float smoothSpeed = 0.125f; // How "laggy" the camera is (0 is instant, 1 is slow)
    public Vector3 offset; // Keeps the camera Z axis correct (-10)

    void LateUpdate() // LateUpdate runs after the player has finished moving
    {
        if (target != null)
        {
            // Calculate where the camera WANTS to be (Player position + offset)
            Vector3 desiredPosition = target.position + offset;

            // Smoothly move from current position to desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Apply the new position
            transform.position = smoothedPosition;
        }
    }
}