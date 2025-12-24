using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    [Header("Dash Settings")]
    public float moveSpeed = 5f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;

    // Define your colors here
    public Color dashColor = Color.cyan;   // Color while dashing
    public Color emptyColor = Color.gray;  // Color when cooldown starts (0%)
    public Color fullColor = Color.white;  // Color when cooldown finishes (100%)

    private Vector2 movement;
    private bool isDashing;
    private bool canDash = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Find the sprite renderer in children (fixes your previous error)
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
            rb.MovePosition(rb.position + movement * dashSpeed * Time.fixedDeltaTime);
        else
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;

        // 1. PHASE: DASHING (Instant Color)
        //spriteRenderer.color = dashColor;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        // 2. PHASE: RECHARGING (Animated Color)
        float timer = 0f;
        while (timer < dashCooldown)
        {
            timer += Time.deltaTime;

            // This math calculates the percentage (0.0 to 1.0)
            float percentage = timer / dashCooldown;

            // "Lerp" blends the colors based on that percentage
            // It creates a smooth fade from Gray -> White
            spriteRenderer.color = Color.Lerp(emptyColor, fullColor, percentage);

            yield return null; // Wait for the next frame
        }

        // 3. PHASE: READY
        spriteRenderer.color = fullColor; // Ensure it's perfectly white at the end
        canDash = true;
    }
}