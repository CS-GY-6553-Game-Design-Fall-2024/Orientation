using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("=== Player Avatar References ===")]
    [SerializeField] private CameraFader m_startCameraFader;
    [SerializeField] private CameraFader m_playerCameraFader;
    [SerializeField] private GameObject m_playerAvatar;
    [SerializeField] private PlayerHealthAndStamina m_playerHS;
    [SerializeField] private PlayerInteraction m_playerInteraction;
    [SerializeField] private GameTracker m_gameTracker;
    [SerializeField] private GameObject m_destinationRef;

    [Header("=== UI References ===")]
    [SerializeField] private CanvasGroup m_startMenuGroup;
    [SerializeField] private CanvasGroup m_winMenuGroup;
    [SerializeField] private CanvasGroup m_downloadGroup;
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

        // Disable the start menu group as well as the win and lose screens
        ToggleCanvasGroup(m_startMenuGroup, false);
        ToggleCanvasGroup(m_winMenuGroup, false);
        ToggleCanvasGroup(m_loseMenuGroup, false);
        ToggleCanvasGroup(m_inGameMenuGroup, false);

        // activate the player
        m_startCameraFader.gameObject.SetActive(false);
        m_destinationRef.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        m_playerAvatar.SetActive(true);
        m_playerHS.ResetHealthAndStamina();
        m_playerInteraction.ResetInGameMenuState();
        m_playerAvatar.GetComponent<FirstPersonMovement>().TogglePlayerMovement(true);
        m_gameTracker.StartTracking();
        m_isPlaying = true;
    }

    // Functionally similar to StartGame, but we do not get the start and end again.
    public void ResetGame() {        
        // Place the player at the start, and the destination prefab at the destination
        m_playerAvatar.transform.position = new Vector3(m_playerStart.x, m_playerStart.y+1f, m_playerStart.z);
        m_destinationRef.transform.position = m_playerDestination;

        // Initialize lighting manager
        m_lightingManager.enabled = true;

        // Disable the start menu group as well as the win and lose screens
        ToggleCanvasGroup(m_startMenuGroup, false);
        ToggleCanvasGroup(m_winMenuGroup, false);
        ToggleCanvasGroup(m_loseMenuGroup, false);
        ToggleCanvasGroup(m_inGameMenuGroup, false);

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
        ToggleCanvasGroup(m_startMenuGroup, true);
        ToggleCanvasGroup(m_winMenuGroup, false);
        ToggleCanvasGroup(m_loseMenuGroup, false);
        ToggleCanvasGroup(m_inGameMenuGroup, false);
        Cursor.lockState = CursorLockMode.None;
        m_isPlaying = false;
    }

    public void ShowWinMenu() {
        if (!m_isPlaying) return;
        m_playerAvatar.GetComponent<FirstPersonMovement>().TogglePlayerMovement(false);
        m_gameTracker.StopTracking();
        bool savingSupported = WebGLFileSaver.IsSavingSupported();
        ToggleCanvasGroup(m_downloadGroup, savingSupported);
        ToggleCanvasGroup(m_winMenuGroup, true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowLoseScreen() {
        if (!m_isPlaying) return;
        m_playerAvatar.GetComponent<FirstPersonMovement>().TogglePlayerMovement(false);
        ToggleCanvasGroup(m_loseMenuGroup, true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void ToggleInGameMenu(bool setTo) {
        if (!m_isPlaying) return;
        int alpha = setTo ? 1 : 0;
        CursorLockMode cursorMode = setTo ? CursorLockMode.None : CursorLockMode.Locked;
        m_playerAvatar.GetComponent<FirstPersonMovement>().TogglePlayerMovement(!setTo);
        ToggleCanvasGroup(m_inGameMenuGroup, setTo);
        Cursor.lockState = cursorMode;
    }

    public void ToggleCanvasGroup(CanvasGroup group, bool setTo) {
        float setToFloat = setTo ? 1f : 0f;
        group.alpha = setTo ? 1f : 0f;
        group.interactable = setTo;
        group.blocksRaycasts = setTo;
    }
}
