using UnityEngine;

public class MapPainter : MonoBehaviour
{
    public RenderTexture mapMemory; // Drag your 'MapMemory' here
    public Material drawMaterial;   // We will make this next
    public float brushSize = 0.05f;

    void Update()
    {
        // Convert world position to a 0-1 range for the texture
        // Assuming your map is roughly 100x100 units. Adjust math as needed.
        Vector2 drawPos = new Vector2(transform.position.x / 100f + 0.5f, transform.position.z / 100f + 0.5f);

        drawMaterial.SetVector("_DrawPos", drawPos);
        drawMaterial.SetFloat("_BrushSize", brushSize);

        // This "paints" the position onto our permanent Render Texture
        RenderTexture temp = RenderTexture.GetTemporary(mapMemory.width, mapMemory.height);
        Graphics.Blit(mapMemory, temp);
        Graphics.Blit(temp, mapMemory, drawMaterial);
        RenderTexture.ReleaseTemporary(temp);
    }
}