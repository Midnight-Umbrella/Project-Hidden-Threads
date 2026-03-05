using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    private static AudioController _instance;
    public static AudioController Instance { get { return _instance; } }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("Clips")]
    [SerializeField] private AudioClip uiClickClip;

    [Header("Scene Music")]
    [SerializeField] private List<SceneMusic> sceneMusics;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // Assign mixer groups to audio sources
        if (musicSource != null && musicGroup != null)
            musicSource.outputAudioMixerGroup = musicGroup;
        if (sfxSource != null && sfxGroup != null)
            sfxSource.outputAudioMixerGroup = sfxGroup;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var musicEntry = sceneMusics.FirstOrDefault(sm => sm.sceneName == scene.name);
        if (musicEntry != null && musicEntry.musicClip != null)
        {
            // Only play if it's a different clip
            if (musicSource.clip != musicEntry.musicClip)
            {
                PlayMusic(musicEntry.musicClip);
            }
            // If same clip, continue playing without restart
        }
        else
        {
            // Stop music if no clip assigned for this scene
            if (musicSource != null)
            {
                musicSource.Stop();
            }
        }
    }

    public void SetMasterVolume(float value)
    {
        if (SoundMixerManager.Instance != null)
        {
            SoundMixerManager.Instance.SetMasterVolume(value);
        }
        else
        {
            AudioListener.volume = value;
        }
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource == null || clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = 1f; // Volume controlled by mixer
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null) return;
        sfxSource.PlayOneShot(clip, 1f); // Volume controlled by mixer
    }

    public void PlaySFXOnSource(AudioSource source, AudioClip clip, float volumeMultiplier = 1f)
    {
        if (source == null || clip == null) return;
        source.PlayOneShot(clip, volumeMultiplier); // Volume multiplier applied directly
    }

    public void PlaySFXAtPosition(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;

        // Create a temporary GameObject with AudioSource
        GameObject tempAudioObject = new GameObject("TempSFX");
        tempAudioObject.transform.position = position;

        AudioSource tempSource = tempAudioObject.AddComponent<AudioSource>();
        tempSource.clip = clip;
        tempSource.volume = volume;
        tempSource.spatialBlend = 1f; // 3D sound
        if (sfxGroup != null)
            tempSource.outputAudioMixerGroup = sfxGroup;
        tempSource.Play();

        // Destroy after clip finishes
        Destroy(tempAudioObject, clip.length);
    }

    public void PlaySFX_2(AudioClip clip, Transform spawnTransform, float volume)
    {
        // Spawns in gameObject
        AudioSource audioSource = Instantiate(sfxSource, spawnTransform.position, Quaternion.identity);
        // Assigns audioClip
        audioSource.clip = clip;
        // Assigns volume
        audioSource.volume = volume;
        // Plays sound
        audioSource.Play();
        // Gets length of clip
        float clipLength = audioSource.clip.length;
        // Destroys gameObject
        Destroy(audioSource.gameObject, clipLength);

    }

    // Convenience for UI buttons
    public void PlayUIClick()
    {
        PlaySFX(uiClickClip);
    }
}

[System.Serializable]
public class SceneMusic
{
    public string sceneName;
    public AudioClip musicClip;
}