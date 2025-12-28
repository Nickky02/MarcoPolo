using UnityEngine;

public class RippleEffect : MonoBehaviour
{
    public float speed = 10f;
    public float maxSize = 20f;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        transform.localScale = Vector3.zero; // Start at zero size
    }

    void Update()
    {
        // Expand the circle
        transform.localScale += Vector3.one * speed * Time.deltaTime;

        // Fade out the color
        Color c = sr.color;
        c.a -= Time.deltaTime * (speed / maxSize);
        sr.color = c;

        // Destroy when too big or invisible
        if (transform.localScale.x >= maxSize || sr.color.a <= 0)
            Destroy(gameObject);
    }
}