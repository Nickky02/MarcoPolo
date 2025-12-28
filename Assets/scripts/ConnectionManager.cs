using UnityEngine;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviour
{
    public GameObject promptUI;
    public GameObject resultUI;
    public float holdTimeRequired = 1.0f;
    private float holdTimer = 0f;
    private bool inRange = false;
    //public GameObject progressUI;


    private void Start()
    {
        //progressUI.SetActive(false);
    }
    void Update()
    {
        if (inRange)
        {
            if (Input.GetKey(KeyCode.E))
            {
                holdTimer += Time.deltaTime;
                if (holdTimer >= holdTimeRequired)
                {
                    ConnectSuccess();
                }
            }
            else
            {
                holdTimer = 0f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Polo"))
        {
            inRange = true;
            promptUI.SetActive(true);
            //progressUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Polo"))
        {
            inRange = false;
            promptUI.SetActive(false);
            //progressUI.SetActive(false);
            holdTimer = 0f;
        }
    }

    void ConnectSuccess()
    {
        promptUI.SetActive(false);
        resultUI.SetActive(true);
        Invoke(nameof(HideResult), 2f);
    }

    void HideResult()
    {
        resultUI.SetActive(false);
    }
}