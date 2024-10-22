using UnityEngine;

public class CameraFOVEffect : MonoBehaviour
{
    [SerializeField] private float normalFOV = 60f;            // The default FOV
    [SerializeField] private float maxFOV = 62f;               // Maximum FOV when breathing (expansion)
    [SerializeField] private float movementFOV = 65f;          // FOV when the player is moving
    [SerializeField] private float breathingSpeed = 10f;       // Speed at which the FOV grows and shrinks
    [SerializeField] private float moveLerpSpeed = 5f;         // Speed of FOV transition during movement
    [SerializeField] private PlayerHealthAndStamina playerStamina;  // Reference to the player's stamina
    [SerializeField] private FirstPersonMovement playerMovement;    // Reference to player's movement script

    public GameObject breath;
    private Camera playerCamera;
    private bool isRegeneratingStamina = false;   // Flag to check if the player is resting
    private float breathingTime = 0f;             // Used to animate the breathing effect

    private void Start()
    {
        // Get the camera component
        playerCamera = GetComponent<Camera>();
        playerCamera.fieldOfView = normalFOV;  // Set the default FOV
    }

    private void Update()
    {
        // Check if the player's stamina is regenerating
        isRegeneratingStamina = playerStamina.m_isRegeneratingStamina && playerStamina.m_currentStamina < playerStamina.m_maxStamina * 0.3f;

        // Check if the player is moving
        bool isMoving = playerMovement.IsPlayerMoving();

        // Handle FOV based on movement or stamina regeneration
        if (isMoving)
        {
            // Smoothly transition to the movement FOV (62)
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, movementFOV, Time.deltaTime * moveLerpSpeed);

            breath.SetActive(false);
        }
        else if (isRegeneratingStamina)
        {
            // Increase breathing time over time to make the FOV oscillate
            breathingTime += Time.deltaTime * breathingSpeed;

            // Create a breathing effect by oscillating the FOV between normalFOV and maxFOV
            float targetFOV = normalFOV + Mathf.Sin(breathingTime) * (maxFOV - normalFOV) * 0.5f;

            // Apply the target FOV to the camera
            playerCamera.fieldOfView = targetFOV;

            breath.SetActive(true);
        }
        else
        {
            // Reset the FOV to normal when neither moving nor resting
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, Time.deltaTime * moveLerpSpeed);
            breathingTime = 0f;  // Reset the breathing time when not resting
            breath.SetActive(false);
        }
    }
}
