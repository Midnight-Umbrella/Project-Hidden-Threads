using UnityEngine;

public class AudioController : MonoBehaviour
{
    private static AudioController _instance;
    public static AudioController Instance { get { return _instance; } }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip uiClickClip;

    private float masterVolume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        AudioListener.volume = masterVolume;
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource == null || clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = masterVolume;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null) return;
        sfxSource.PlayOneShot(clip, masterVolume);
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