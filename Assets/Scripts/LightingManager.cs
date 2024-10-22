using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Header("=== References ===")]
    [SerializeField] private Light m_directionalLight;
    [SerializeField] private Transform m_lightParent;
    [SerializeField] private LightingPreset m_preset;
    [SerializeField] private Material m_skyboxMaterial;

    [Header("=== Variables ===")]
    [SerializeField] private float m_timeScale = 0.5f;
    [SerializeField, Range(0,24)] private float m_timeOfDay; 
    [SerializeField, Range(0,1)] private float m_nightSkyboxTimestamp = .825f;
    [SerializeField, Range(0,1)] private float m_daySkyboxTimestamp = .175f;
    [SerializeField] private AnimationCurve m_skyboxTransparencyCurve;

    private void Update() {
        if (m_preset == null) return;
        m_skyboxMaterial = RenderSettings.skybox;
        if (Application.isPlaying) {
            m_timeOfDay += Time.deltaTime * m_timeScale;
            m_timeOfDay %= 24;
            UpdateLighting(m_timeOfDay);
        }
        else {
            UpdateLighting(m_timeOfDay);
        }
    }

    public void UpdateLighting(float timeOfDay) {
        float timePercent = timeOfDay/24f;
        RenderSettings.ambientLight = m_preset.ambientColor.Evaluate(timePercent);
        RenderSettings.fogColor = m_preset.fogColor.Evaluate(timePercent);

        if (m_directionalLight != null) {
            m_directionalLight.color = m_preset.directionalColor.Evaluate(timePercent);
        }
        if (m_lightParent != null) {
            m_lightParent.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f)-90f, 170f, 0f));
        }

        if (m_skyboxMaterial == null) return;
        if (timePercent >= m_nightSkyboxTimestamp || timePercent < m_daySkyboxTimestamp) m_skyboxMaterial.SetTexture("_Tex", (Texture)m_preset.nightSkyboxMat);
        else m_skyboxMaterial.SetTexture("_Tex", (Texture)m_preset.daySkyboxMat);
        float skyboxExposure = m_skyboxTransparencyCurve.Evaluate(timePercent);
        m_skyboxMaterial.SetFloat("_Exposure", skyboxExposure);
    }

    private void OnValidate() {
        if (m_directionalLight != null) return;
        if (RenderSettings.sun != null) {
            m_directionalLight = RenderSettings.sun;
        }
        else {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach(Light light in lights) {
                if (light.type == LightType.Directional) {
                    m_directionalLight = light;
                    return;
                }
            }
        }
    }
}
