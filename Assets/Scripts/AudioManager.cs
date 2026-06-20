using System;
using UnityEngine;

/// <summary>
/// Handles audio playback (SFX + Music)
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioSource;   // SFX
    [SerializeField] private AudioSource musicSource;   // Music

    [Header("Audio Clips")]
    [SerializeField] private AudioClip errorClip;
    [SerializeField] private AudioClip menuClip;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 🔥 IMPORTANT: Ensure TWO different AudioSources
        AudioSource[] sources = GetComponents<AudioSource>();

        if (sources.Length == 0)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            musicSource = gameObject.AddComponent<AudioSource>();
        }
        else if (sources.Length == 1)
        {
            audioSource = sources[0];
            musicSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            audioSource = sources[0];
            musicSource = sources[1];
        }

        // ✅ SAFE: null check before config
        if (audioSource != null)
            audioSource.playOnAwake = false;

        if (musicSource != null)
        {
            musicSource.playOnAwake = false;
            musicSource.loop = true;
        }
    }

    private void Start()
    {
        // Keep this simple
        PlayMenu();
    }

    public void PlayError()
    {
        // ✅ SAFE: prevent null crash
        if (audioSource == null || errorClip == null) return;

        audioSource.PlayOneShot(errorClip);
    }

    public void PlayMenu()
    {
        // ✅ SAFE: prevent null crash
        if (musicSource == null || menuClip == null) return;

        // 🔥 Only set clip if different
        if (musicSource.clip != menuClip)
        {
            musicSource.clip = menuClip;
        }

        // 🔥 Only play if not already playing
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void StopMenu()
    {
        Debug.Log("stop called");

        // ✅ SAFE: prevent null crash
        if (musicSource == null) return;

        musicSource.Stop();
    }
}