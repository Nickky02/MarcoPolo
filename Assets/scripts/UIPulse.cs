using UnityEngine;

public class UIPulse : MonoBehaviour
{
    public float speed = 2f;
    public float scaleAmount = 0.05f;

    Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        float s = 1 + Mathf.Sin(Time.time * speed) * scaleAmount;
        transform.localScale = startScale * s;
    }
}
