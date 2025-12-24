using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    // New variables for limits
    public Vector2 minPosition; // Bottom-Left limit
    public Vector2 maxPosition; // Top-Right limit

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;

            // 1. Smooth the movement first
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // 2. Clamp (Lock) the position inside the boundaries
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minPosition.x, maxPosition.x);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minPosition.y, maxPosition.y);

            transform.position = smoothedPosition;
        }
    }
}