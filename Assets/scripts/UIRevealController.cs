//using UnityEngine;
//using UnityEngine.UI;

//public class UIRevealController : MonoBehaviour
//{
//    [Header("Setup")]
//    public Image colorImage;

//    [Header("Settings")]
//    public float radius = 200f; // Keep this at 200. In Object Space, 1 unit = 1 pixel.
//    public float softness = 100f;

//    private Material instancedMaterial;
//    private RectTransform rectTransform;

//    void Start()
//    {
//        if (colorImage != null)
//        {
//            instancedMaterial = colorImage.material;
//            rectTransform = colorImage.GetComponent<RectTransform>();
//        }
//    }

//    void Update()
//    {
//        if (instancedMaterial == null || rectTransform == null) return;

//        // 1. Get Mouse Position relative to the Image's center
//        Vector2 localPoint;
//        RectTransformUtility.ScreenPointToLocalPointInRectangle(
//            rectTransform,          // The UI Element
//            Input.mousePosition,    // The Mouse
//            null,                   // Camera (Null is required for 'Screen Space - Overlay')
//            out localPoint          // The Result
//        );

//        // 2. Send the local coordinates (e.g., -400, 250) to the shader
//        instancedMaterial.SetVector("_MousePos", localPoint);
//        instancedMaterial.SetFloat("_Radius", radius);
//        instancedMaterial.SetFloat("_Softness", softness);
//    }
//}

using UnityEngine;
using UnityEngine.UI;

public class UIRevealController : MonoBehaviour
{
    [Header("Setup")]
    public Image colorImage;
    public RectTransform customCursor; // Drag your "+" object here

    [Header("Settings")]
    public float radius = 200f;
    public float softness = 100f;
    [Range(0.01f, 0.5f)] public float smoothTime = 0.05f; // Higher = More "Buttery/Drunk"

    private Material instancedMaterial;
    private RectTransform rectTransform;

    // Variables for smoothing
    private Vector2 _smoothedPos;
    private Vector2 _velocity;

    void Start()
    {
        Cursor.visible = false;

        if (colorImage != null)
        {
            instancedMaterial = colorImage.material;
            rectTransform = colorImage.GetComponent<RectTransform>();
        }

        _smoothedPos = Input.mousePosition;
    }

    void Update()
    {
        if (instancedMaterial == null || rectTransform == null) return;

        _smoothedPos = Vector2.SmoothDamp(_smoothedPos, Input.mousePosition, ref _velocity, smoothTime);

        // 3. Move the "+" Visual
        if (customCursor != null)
        {
            customCursor.position = _smoothedPos;
        }

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            _smoothedPos, // Use smoothed pos here!
            null,
            out localPoint
        );

        instancedMaterial.SetVector("_MousePos", localPoint);
        instancedMaterial.SetFloat("_Radius", radius);
        instancedMaterial.SetFloat("_Softness", softness);
    }
}