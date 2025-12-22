using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Transition Settings")]
    public Image fadePanel;           // Drag your White "FadePanel" here
    public Image backgroundToDistort; // Drag your Background_Color image here
    public float transitionTime = 1.5f;
    public float maxLensDistortion = 0.0f; // How "curved" the sphere gets

    public void PlayGame()
    {
        // Start the animation routine
        StartCoroutine(TransitionRoutine());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator TransitionRoutine()
    {
        // 1. Get the material instance
        Material mat = backgroundToDistort.material;
        float timer = 0f;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            float progress = timer / transitionTime;

            // 2. Animate the Lens Distortion (Exponential feels better)
            // We use 'progress * progress' so it starts slow and zooms fast at the end
            float currentDistortion = Mathf.Lerp(0, maxLensDistortion, progress * progress);
            mat.SetFloat("_LensPower", currentDistortion);

            // 3. Animate the White Fade (Alpha 0 to 1)
            if (fadePanel != null)
            {
                Color c = fadePanel.color;
                c.a = progress; // 0 to 1
                fadePanel.color = c;
            }

            yield return null; // Wait for next frame
        }

        // 4. Load the scene
        SceneManager.LoadScene("OutdoorsScene");
    }
}