using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    // Referring to WASD movement requirement 
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;

    [Header("Fatigue Settings")]
    [Tooltip("Speed during the split-second exhaustion after a dash")]
    public float fatigueSpeed = 2f;
    public float fatigueDuration = 0.3f;

    [Header("Visuals")]
    public Color dashColor = Color.cyan;
    public Color emptyColor = Color.gray;
    public Color fullColor = Color.white;

    private Vector2 movement;
    private bool isDashing;
    private bool isFatigued;
    private bool canDash = true;


    [Header("Honk Settings")]
    public float honkRadius = 8f;
    public LayerMask poloLayer; // Set this to the layer Polo is on
    public GameObject rippleEffectPrefab; // A simple circle that expands

    public GameObject poloHintUI; // Drag your UI text here
    public Transform poloTransform; // Drag the Polo_Ghost parent here

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
       // WASD movement input [cite: 4, 11]
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && movement != Vector2.zero)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Honk();
        }


    }

    void FixedUpdate()
    {
        if (movement == Vector2.zero)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float speed = moveSpeed;

        if (isDashing) speed = dashSpeed;
        else if (isFatigued) speed = fatigueSpeed;

        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        isFatigued = true;

        float timer = 0f;
        while (timer < dashCooldown)
        {
            timer += Time.deltaTime;

            if (timer >= fatigueDuration)
            {
                isFatigued = false;
            }

            float percentage = timer / dashCooldown;
            spriteRenderer.color = Color.Lerp(emptyColor, fullColor, percentage);

            yield return null;
        }

        spriteRenderer.color = fullColor;
        canDash = true;
    }
    void Honk()
    {
        // Player calls out Marco
        Debug.Log("-Marco");

        // Visual feedback
        if (rippleEffectPrefab != null)
            Instantiate(rippleEffectPrefab, transform.position, Quaternion.identity);

        // Check for Polo in range
        Collider2D hit = Physics2D.OverlapCircle(transform.position, honkRadius, poloLayer);

        Debug.Log(hit ? hit.name : "No hit");

        if (hit != null)
        {
            // NPC responds Polo
            Debug.Log("Polo-");

            // 2. Check if Polo is off-screen
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(poloTransform.position);
            bool isOffScreen = screenPoint.z < 0 || screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1;

            if (isOffScreen)
            {
                StartCoroutine(ShowPoloHint(screenPoint));
            }
        }
        IEnumerator ShowPoloHint(Vector3 screenPos)
        {
            poloHintUI.SetActive(true);

            // 1. Get the center of the screen
            Vector2 screenCenter = new Vector2(Screen.width, Screen.height) * 0.5f;

            // 2. Get the screen position of Polo (this is in Pixels, not 0-1)
            Vector3 poloScreenPos = Camera.main.WorldToScreenPoint(poloTransform.position);

            // 3. Calculate direction from center to Polo
            Vector2 dir = ((Vector2)poloScreenPos - screenCenter).normalized;

            // 4. Project that direction to the edge of the screen
            // We use a padding (e.g., 50 pixels) so it's not cut off by the monitor edge
            float padding = 50f;
            float xEdge = (Screen.width * 0.5f) - padding;
            float yEdge = (Screen.height * 0.5f) - padding;

            // This math finds which edge (top/bottom or left/right) the direction hits first
            float m = dir.y / dir.x;
            Vector3 finalPos;

            if (Mathf.Abs(dir.x * yEdge) > Mathf.Abs(dir.y * xEdge))
            {
                // Hits left or right edge
                finalPos = new Vector3(Mathf.Sign(dir.x) * xEdge, Mathf.Sign(dir.x) * xEdge * m, 0);
            }
            else
            {
                // Hits top or bottom edge
                finalPos = new Vector3(Mathf.Sign(dir.y) * yEdge / m, Mathf.Sign(dir.y) * yEdge, 0);
            }

            // 5. Offset back to screen coordinates
            poloHintUI.transform.position = finalPos + (Vector3)screenCenter;

            // 6. Optional: Rotate the hint to point at Polo
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            poloHintUI.transform.rotation = Quaternion.Euler(0, 0, angle);

            yield return new WaitForSeconds(2f);
            poloHintUI.SetActive(false);
        }
    }

}