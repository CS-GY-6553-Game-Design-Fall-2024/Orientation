using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarUI : MonoBehaviour
{
    [SerializeField] private Image staminaBarFill;
    [SerializeField] private PlayerHealthAndStamina playerStamina;
    [SerializeField] private float lerpSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        float targetFillAmount = playerStamina.m_currentStamina / playerStamina.m_maxStamina;
        staminaBarFill.fillAmount = Mathf.Lerp(staminaBarFill.fillAmount, targetFillAmount, Time.deltaTime * lerpSpeed);
        //Debug.Log(targetFillAmount);
    }
}
