using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("=== Player Avatar References ===")]
    [SerializeField] private CameraFader m_startCameraFader;
    [SerializeField] private CameraFader m_playerCameraFader;
    [SerializeField] private GameObject m_playerAvatar;
    [SerializeField] private PlayerHealthAndStamina m_playerHS;
    [SerializeField] private PlayerInteraction m_playerInteraction;
    [SerializeField] private GameObject m_destinationRef;

    [Header("=== UI References ===")]
    [SerializeField] private CanvasGroup m_startMenuGroup;
    [SerializeField] private CanvasGroup m_winMenuGroup;
    [SerializeField] private CanvasGroup m_loseMenuGroup;
    [SerializeField] private CanvasGroup m_inGameMenuGroup;

    [Header("=== Generation Engines ===")]
    [SerializeField] private TerrainGenerator m_terrainGenerator;
    [SerializeField] private LightingManager m_lightingManager;

    [Header("=== Settings ===")]
    [SerializeField] private float m_minDistanceBetweenStartAndEnd = 20f;
    [SerializeField] private Vector3 m_playerStart;
    [SerializeField] private Vector3 m_playerDestination;
    [SerializeField] private bool m_isPlaying = false;

    private void Start() {
        GoToStart();
    }

    public void StartGame() {
        // Set the start and end destinations for the player
        m_terrainGenerator.GetStartAndEnd(m_minDistanceBetweenStartAndEnd, out m_playerStart, out m_playerDestination);
        
        // Place the player at the start, and the destination prefab at the destination
        m_playerAvatar.transform.position = new Vector3(m_playerStart.x, m_playerStart.y+1f, m_playerStart.z);
        m_destinationRef.transform.position = m_playerDestination;

        // Initialize lighting manager
        m_lightingManager.enabled = true;

        // Disable the start menu group
        m_startMenuGroup.alpha = 0;
        m_startMenuGroup.interactable = false;
        m_startMenuGroup.blocksRaycasts = false;

        // Disable the win and lose screens, if necessary
        m_winMenuGroup.alpha = 0;
        m_winMenuGroup.interactable = false;
        m_winMenuGroup.blocksRaycasts = false;
        m_loseMenuGroup.alpha = 0;
        m_loseMenuGroup.interactable = false;
        m_loseMenuGroup.blocksRaycasts = false;
        m_inGameMenuGroup.alpha = 0;
        m_inGameMenuGroup.interactable = false;
        m_inGameMenuGroup.blocksRaycasts = false;

        // activate the player
        m_startCameraFader.gameObject.SetActive(false);
        m_destinationRef.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        m_playerAvatar.SetActive(true);
        m_playerHS.ResetHealthAndStamina();
        m_playerInteraction.ResetInGameMenuState();
        m_playerAvatar.GetComponent<FirstPersonMovement>().TogglePlayerMovement(true);
        m_isPlaying = true;
    }

    // Functionally similar to StartGame, but we do not get the start and end again.
    public void ResetGame() {        
        // Place the player at the start, and the destination prefab at the destination
        m_playerAvatar.transform.position = new Vector3(m_playerStart.x, m_playerStart.y+1f, m_playerStart.z);
        m_destinationRef.transform.position = m_playerDestination;

        // Initialize lighting manager
        m_lightingManager.enabled = true;

        // Disable the start menu group
        m_startMenuGroup.alpha = 0;
        m_startMenuGroup.interactable = false;
        m_startMenuGroup.blocksRaycasts = false;

        // Disable the win and lose screens, if necessary
        m_winMenuGroup.alpha = 0;
        m_winMenuGroup.interactable = false;
        m_winMenuGroup.blocksRaycasts = false;
        m_loseMenuGroup.alpha = 0;
        m_loseMenuGroup.interactable = false;
        m_loseMenuGroup.blocksRaycasts = false;
        m_inGameMenuGroup.alpha = 0;
        m_inGameMenuGroup.interactable = false;
        m_inGameMenuGroup.blocksRaycasts = false;

        // activate the player
        m_startCameraFader.gameObject.SetActive(false);
        m_destinationRef.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        m_playerAvatar.SetActive(true);
        m_playerHS.ResetHealthAndStamina();
        m_playerInteraction.ResetInGameMenuState();
        m_playerAvatar.GetComponent<FirstPersonMovement>().TogglePlayerMovement(true);
        m_isPlaying = true;
    }

    public void GoToStart() {
        m_playerAvatar.SetActive(false);
        m_playerAvatar.GetComponent<FirstPersonMovement>().TogglePlayerMovement(false);
        m_startCameraFader.gameObject.SetActive(true);
        m_destinationRef.SetActive(false);
        m_lightingManager.UpdateLighting(10f);
        m_lightingManager.enabled = false;
        m_startMenuGroup.alpha = 1;
        m_startMenuGroup.interactable = true;
        m_startMenuGroup.blocksRaycasts = true;
        m_winMenuGroup.alpha = 0;
        m_winMenuGroup.interactable = false;
        m_winMenuGroup.blocksRaycasts = false;
        m_loseMenuGroup.alpha = 0;
        m_loseMenuGroup.interactable = false;
        m_loseMenuGroup.blocksRaycasts = false;
        m_inGameMenuGroup.alpha = 0;
        m_inGameMenuGroup.interactable = false;
        m_inGameMenuGroup.blocksRaycasts = false;
        Cursor.lockState = CursorLockMode.None;
        m_isPlaying = false;
    }

    public void ShowWinMenu() {
        if (!m_isPlaying) return;
        m_playerAvatar.GetComponent<FirstPersonMovement>().TogglePlayerMovement(false);
        m_winMenuGroup.alpha = 1;
        m_winMenuGroup.interactable = true;
        m_winMenuGroup.blocksRaycasts = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowLoseScreen() {
        if (!m_isPlaying) return;
        m_playerAvatar.GetComponent<FirstPersonMovement>().TogglePlayerMovement(false);
        m_loseMenuGroup.alpha = 1;
        m_loseMenuGroup.interactable = true;
        m_loseMenuGroup.blocksRaycasts = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ToggleInGameMenu(bool setTo) {
        if (!m_isPlaying) return;
        int alpha = setTo ? 1 : 0;
        CursorLockMode cursorMode = setTo ? CursorLockMode.None : CursorLockMode.Locked;
        m_playerAvatar.GetComponent<FirstPersonMovement>().TogglePlayerMovement(!setTo);
        m_inGameMenuGroup.alpha = alpha;
        m_inGameMenuGroup.interactable = setTo;
        m_inGameMenuGroup.blocksRaycasts = setTo;
        Cursor.lockState = cursorMode;
    }
}
