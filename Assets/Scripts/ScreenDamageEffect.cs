using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenDamageEffect : MonoBehaviour
{
    [SerializeField] private Image redScreenOverlay;  // Reference to the red screen overlay
    [SerializeField] private float fadeSpeed = 2f;    // Speed of the fade-out effect
    [SerializeField] private float maxAlpha = 0.5f;   // Maximum alpha value for the flash effect

    private Color redColor;
    private bool isFading = false;

    private void Start()
    {
        // Initialize the color and set alpha to 0
        redColor = redScreenOverlay.color;
        redColor.a = 0;
        redScreenOverlay.color = redColor;
    }

    private void Update()
    {
        // If the red overlay is fading, gradually reduce its alpha
        if (isFading)
        {
            redColor.a -= Time.deltaTime * fadeSpeed;
            redScreenOverlay.color = redColor;

            // Stop fading when alpha reaches 0
            if (redColor.a <= 0)
            {
                isFading = false;
                redColor.a = 0;
                redScreenOverlay.color = redColor;
            }
        }
    }

    // Call this method when the player takes damage
    public void TriggerRedEffect()
    {
        redColor.a = maxAlpha;  // Set alpha to maximum to show the red flash
        redScreenOverlay.color = redColor;
        isFading = true;        // Start fading out the red overlay
    }
}
