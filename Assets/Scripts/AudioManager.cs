using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("=== BGM Settings ===")]
    [SerializeField] private AudioSource bgmSource;  // The AudioSource for background music
    [SerializeField] private AudioClip bgmClip;      // Background music clip

    [Header("=== Ambient Sound Settings ===")]
    [SerializeField] private AudioSource ambientSource;  // The AudioSource for ambient sound
    [SerializeField] private AudioClip ambientClip;      // Ambient sound clip

    private void Start()
    {
        // Play both BGM and Ambient sound when the game starts
        PlayBGM();
        PlayAmbientSound();
    }

    public void PlayBGM()
    {
        if (bgmSource != null && bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void PlayAmbientSound()
    {
        if (ambientSource != null && ambientClip != null)
        {
            ambientSource.clip = ambientClip;
            ambientSource.loop = true;
            ambientSource.Play();
        }
    }

    // Optional: Stop BGM or Ambient Sound if needed
    public void StopBGM()
    {
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
    }

    public void StopAmbientSound()
    {
        if (ambientSource != null && ambientSource.isPlaying)
        {
            ambientSource.Stop();
        }
    }
}
