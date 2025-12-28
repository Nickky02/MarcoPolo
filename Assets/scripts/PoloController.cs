using UnityEngine;
using System.Collections;

public class PoloController : MonoBehaviour
{
    [Header("Flight Settings")]
    public Transform[] areas; // Drag Target1-5 here
    public float flySpeed = 2.5f;

    [Header("Animation")]
    public Transform visualChild; // Drag the 'Visual' child object here
    public float bobHeight = 0.4f;
    public float bobSpeed = 2.5f;

    private Vector3 nextDestination;
    //private bool isFlying = false;

    void Start()
    {
        // Start by picking a random spot from your 5 areas
        nextDestination = areas[Random.Range(0, areas.Length)].position;
        StartCoroutine(FlightTimer());
    }

    void Update()
    {
        // Move the Parent object toward the area
        transform.position = Vector3.MoveTowards(transform.position, nextDestination, flySpeed * Time.deltaTime);

        // Make the Child sprite bob up and down independently
        float newY = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        visualChild.localPosition = new Vector3(0, newY, 0);
    }

    IEnumerator FlightTimer()
    {
        while (true)
        {
            // Wait at the current spot for 3 to 7 seconds
            yield return new WaitForSeconds(Random.Range(3f, 7f));
            // Choose a new random area to fly to
            nextDestination = areas[Random.Range(0, areas.Length)].position;
        }
    }
}