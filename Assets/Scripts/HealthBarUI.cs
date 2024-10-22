using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private PlayerHealthAndStamina playerHealth;
    [SerializeField] private float lerpSpeed = 5f;

    private void Update()
    {
        float targetFillAmount = playerHealth.m_currentHealth / playerHealth.m_maxHealth;
        healthBarFill.fillAmount = Mathf.Lerp(healthBarFill.fillAmount, targetFillAmount, Time.deltaTime * lerpSpeed);
        //Debug.Log(healthBarFill.fillAmount);
    }
}
