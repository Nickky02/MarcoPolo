using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Control speed here
    public Rigidbody2D rb;

    Vector2 movement;

    void Update()
    {
        // 1. Listen for keyboard input (WASD or Arrow Keys)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // 2. Normalize ensures diagonal movement isn't faster than straight movement
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        // 3. Move the character physically
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}