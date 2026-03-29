using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    private static SoundMixerManager _instance;
    public static SoundMixerManager Instance { get { return _instance; } }

    [SerializeField] private AudioMixer audioMixer;

    private const string MasterPref = "MasterVolume";
    private const string MusicPref = "MusicVolume";
    private const string SFXPref = "SFXVolume";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // Load player preferences
        SetMasterVolume(PlayerPrefs.GetFloat(MasterPref, 1f));
        SetMusicVolume(PlayerPrefs.GetFloat(MusicPref, 1f));
        SetSFXVolume(PlayerPrefs.GetFloat(SFXPref, 1f));
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat(MasterPref, value);
        PlayerPrefs.Save();
    }
    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat(MusicPref, value);
        PlayerPrefs.Save();
    }
    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat(SFXPref, value);
        PlayerPrefs.Save();
    }
    
    public float GetMasterVolume()
    {
        audioMixer.GetFloat("MasterVolume", out float value);
        return Mathf.Pow(10, value / 20);
    }
    public float GetMusicVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float value);
        return Mathf.Pow(10, value / 20);
    }
    public float GetSFXVolume()
    {
        audioMixer.GetFloat("SFXVolume", out float value);
        return Mathf.Pow(10, value / 20);
    }
}
