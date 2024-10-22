using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthAndStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    [SerializeField] public float m_maxStamina = 100f;
    [SerializeField] public float m_currentStamina;
    [SerializeField] public float m_staminaRegenRate = 50f;
    [SerializeField] public float m_staminaDepletionRate = 10f;

    [Header("Health Settings")]
    [SerializeField] public float m_maxHealth = 100f;
    [SerializeField] public float m_currentHealth;
    [SerializeField] public float m_fallingDamageThreshold = 10f;

    public bool m_isRegeneratingStamina = true;

    private void Awake()
    {
        m_currentHealth = m_maxHealth;
        m_currentStamina = m_maxStamina;
    }

    public void ResetHealthAndStamina() {
        m_currentHealth = m_maxHealth;
        m_currentStamina = m_maxStamina;
    }
}
