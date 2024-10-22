using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(CharacterController))]
public class FirstPersonMovement : MonoBehaviour
{
    [Header("=== References ===")]
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private Camera m_playerCamera;      // The player's camera (child of the player object)
    [SerializeField] private CharacterController m_characterController;

    [Header("=== Movement Settings ===")]
    [SerializeField] private float m_cameraRotationSpeed = 5f;
    [SerializeField] private float m_movementSpeed = 2f;
    [SerializeField] private float m_jumpForce = 5f;
    [SerializeField] private float m_jumpStamina = 20f;
    private float m_velocityY = 0f;

    [Header("=== Health and Stamina ===")]
    [SerializeField] private PlayerHealthAndStamina m_healthAndStamina;
    [SerializeField] private ScreenDamageEffect screenDamageEffect;

    [Header("=== Audio Settings ===")]
    [SerializeField] private AudioSource jumpAudioSource;  // AudioSource for jump sound
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioSource hurtAudioSource;  // AudioSource for jump sound
    [SerializeField] private AudioClip hurtClip;

    /*
    public float moveSpeed = 5f;        // Speed of movement
    public float mouseSensitivity = 100f; // Mouse sensitivity for looking around
    public float groundCheckDistance = 1.1f; // Distance for ground check using raycast
    private Rigidbody rb;
    private float verticalLookRotation = 0f;
    private bool isGrounded;
    */

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Visualize the raycast for ground detection in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * m_characterController.height/2f);
    }
#endif


    private void Awake() {
        if (m_characterController == null) m_characterController = GetComponent<CharacterController>();
        //Cursor.lockState = CursorLockMode.Locked;

        //reference for health and stamina
        if(m_healthAndStamina == null) m_healthAndStamina = GetComponent<PlayerHealthAndStamina>();
    }

    private void Update() {
        if (!m_characterController.enabled) return;
        Rotation();
        Movement();
    }

    // Manage player rotation
    void Rotation(){
        // We rotate the camera vertically depending on the input of the mouse's y displacement.
        // We use `+=` to maintain camera's previous rotation. `Input.GetAxis` only checks the displacement during the frame.
        m_playerCamera.transform.localEulerAngles += m_cameraRotationSpeed * new Vector3(-Input.GetAxis("Mouse Y"), 0f, 0f);
        // We rotate the player itself when it comes to horizontal rotation. 
        // We use the `up` vector as the axis to rotate around. We rotate based on the mouse x input
        transform.Rotate(transform.up, Input.GetAxis("Mouse X")*m_cameraRotationSpeed);
    }

    // Manage player Movement
    void Movement(){
        // If movement is enabled, then we need to read the player inputs too
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        
        // We start the footstep audio if the movement value is bigger than some threshold
        //if (move.magnitude >= 0.1f) m_footstepSource.mute = false;
        //else m_footstepSource.mute = true;
        move = Vector3.Normalize(transform.TransformDirection(move)) * m_movementSpeed;
        // Apply gravity and/or jump force, if appropriate
        if (!m_characterController.isGrounded) { 
            m_velocityY -= 9.81f * Time.deltaTime;
            if (m_velocityY > -1f && m_velocityY < 0f && Input.GetKeyDown(KeyCode.Space) && m_healthAndStamina.m_currentStamina > m_jumpStamina) {
                m_velocityY = m_jumpForce;
                m_healthAndStamina.m_currentStamina -= m_jumpStamina;
                if (m_healthAndStamina.m_currentStamina < 0) m_healthAndStamina.m_currentStamina = 0;
                m_healthAndStamina.m_isRegeneratingStamina = false;

                jumpAudioSource.PlayOneShot(jumpClip);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && m_healthAndStamina.m_currentStamina > m_jumpStamina) {
            m_velocityY = m_jumpForce;
            m_healthAndStamina.m_currentStamina -= m_jumpStamina;
            if (m_healthAndStamina.m_currentStamina < 0) m_healthAndStamina.m_currentStamina = 0;
            m_healthAndStamina.m_isRegeneratingStamina = false;

            jumpAudioSource.PlayOneShot(jumpClip);
        }
        else {
            if (m_velocityY < -m_healthAndStamina.m_fallingDamageThreshold)
            {  // Set -10f as the threshold for fall damage, you can adjust it
                float fallDamage = Mathf.Abs(m_velocityY) * 2; // Calculate damage based on fall speed
                m_healthAndStamina.m_currentHealth -= fallDamage;  // Apply fall damage
                Debug.Log($"Player took {fallDamage} damage from falling!");
                hurtAudioSource.PlayOneShot(hurtClip);

                if (screenDamageEffect != null)
                {
                    screenDamageEffect.TriggerRedEffect();
                }

                if (m_healthAndStamina.m_currentHealth <= 0f)
                {
                    PlayerDeath();  // Handle player death when health is zero or below
                }
            }
            m_velocityY = 0f;
        }
        

        move += new Vector3(0f, m_velocityY, 0f);

        //stamina deplete
        if (move.magnitude > 1.0f && m_healthAndStamina.m_currentStamina > 0f) { 
            m_healthAndStamina.m_currentStamina -= m_healthAndStamina.m_staminaDepletionRate * Time.deltaTime;
            m_healthAndStamina.m_isRegeneratingStamina = false;
        }

        //stamina regeneration
        if (m_healthAndStamina.m_isRegeneratingStamina && m_healthAndStamina.m_currentStamina < m_healthAndStamina.m_maxStamina) {
            m_healthAndStamina.m_currentStamina += m_healthAndStamina.m_staminaRegenRate * Time.deltaTime;
        }

        //set stamina regenerate state
        if (move.magnitude < 1.0f) { 
            m_healthAndStamina.m_isRegeneratingStamina = true;
        }


        // Commit movement
        if (m_healthAndStamina.m_currentStamina <= 0) {
            move.x = 0;
            move.z = 0;
        }
        m_characterController.Move(move * Time.deltaTime);
    }


    void PlayerDeath()
    {
        Debug.Log("Player has died!");
        // Implement respawn, game over, or other death handling logic
        m_gameManager.ShowLoseScreen();
    }

    public void TogglePlayerMovement(bool setTo) {
        m_characterController.enabled = setTo;
    }

    public bool IsPlayerMoving()
    {
        // Check if the player is moving based on input
        return Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
    }

}
