using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum VolumeType
{
    Master,
    Music,
    SFX
}

public class VolumeWidget : MonoBehaviour
{
    [Header("UI Refs")]
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text valueText; 
    [SerializeField] private VolumeType volumeType;

    private bool _ignore;

    private void Awake()
    {
        // Make sure Volume Initial Volume Accurate 
        VolumeBus.ApplySaved();

        if (slider == null) slider = GetComponentInChildren<Slider>();

        // Initialize UI Display
        float initialValue = 1f;
        switch (volumeType)
        {
            case VolumeType.Master:
                initialValue = SoundMixerManager.Instance.GetMasterVolume();
                break;
            case VolumeType.Music:
                initialValue = SoundMixerManager.Instance.GetMusicVolume();
                break;
            case VolumeType.SFX:
                initialValue = SoundMixerManager.Instance.GetSFXVolume();
                break;
        }

        SetUI(initialValue);

        if (slider != null)
            slider.onValueChanged.AddListener(OnSliderChanged);

        // Change the entire environment -> Update UI (Use “Two-side view/Two-side UI same step”)
        VolumeBus.OnChanged += SetUI;
    }

    private void OnDestroy()
    {
        if (slider != null)
            slider.onValueChanged.RemoveListener(OnSliderChanged);
        VolumeBus.OnChanged -= SetUI;
    }

    private void OnSliderChanged(float v)
    {
        if (_ignore) return;
        switch (volumeType)
        {
            case VolumeType.Master:
                SoundMixerManager.Instance.SetMasterVolume(v);
                break;
            case VolumeType.Music:
                SoundMixerManager.Instance.SetMusicVolume(v);
                break;
            case VolumeType.SFX:
                SoundMixerManager.Instance.SetSFXVolume(v);
                break;
        }

        SetUI(v);
    }

    private void SetUI(float v)
    {
        _ignore = true;
        if (slider != null) slider.SetValueWithoutNotify(v);
        if (valueText != null) valueText.text = $"{Mathf.RoundToInt(v * 100f)}%";
        _ignore = false;
    }
}
