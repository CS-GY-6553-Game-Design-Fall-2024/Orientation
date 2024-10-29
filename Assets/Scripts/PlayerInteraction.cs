using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("=== References ===")]
    [SerializeField] private GameManager m_gameManager;

    [Header("=== Held Map Settings ===")]
    [SerializeField] private Transform m_heldMap;
    [SerializeField] private Transform m_heldMapVisiblePosRef;
    [SerializeField] private Transform m_heldMapInvisiblePosRef;
    [SerializeField] private KeyCode m_showMapKey = KeyCode.LeftShift;
    [SerializeField] private bool m_isShowingMap = false;
    [SerializeField] private float m_heldMapTransitionTime = .1f;
    [SerializeField] private AudioSource mapAudioSource;
    [SerializeField] private AudioClip mapSound;
    private Vector3 m_heldMapVelocity = Vector3.zero;

    [Header("=== Other Settings ===")]
    [SerializeField] private KeyCode m_menuKey = KeyCode.Escape;
    private bool m_prevMenuKeyState = false;

    private void Update()
    {
        m_isShowingMap = Input.GetKey(m_showMapKey);
        if (Input.GetKeyDown(m_showMapKey)) {
            mapAudioSource.PlayOneShot(mapSound);
        }
        Vector3 m_heldMapTarget = (m_isShowingMap) ? m_heldMapVisiblePosRef.position : m_heldMapInvisiblePosRef.position;
        m_heldMap.position = Vector3.SmoothDamp(m_heldMap.position, m_heldMapTarget, ref m_heldMapVelocity, m_heldMapTransitionTime);
        m_heldMap.gameObject.SetActive(m_isShowingMap || Vector3.Distance(m_heldMap.position, m_heldMapInvisiblePosRef.position) >= 0.1f);

        if (Input.GetKeyDown(m_menuKey)) {
            // We toggle the in-game menu 
            m_prevMenuKeyState = !m_prevMenuKeyState;
            m_gameManager.ToggleInGameMenu(m_prevMenuKeyState);
        }
    }

    public void ResetInGameMenuState() {
        m_prevMenuKeyState = false;
    }

    public int CheckHoldingMap() {
        return m_isShowingMap ? 1 : 0;
    }
}

