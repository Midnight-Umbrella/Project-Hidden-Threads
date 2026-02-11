using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingsUI : MonoBehaviour
{
    [Header("UI Refs")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Text volumeValueText;

    [Header("Optional")]
    [SerializeField] private AudioSource targetAudioSource; // if null, use AudioListener

    private const string PrefKey = "master_volume";

    private void Awake()
    {
        float v = PlayerPrefs.GetFloat(PrefKey, 1f);

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.wholeNumbers = false;

            volumeSlider.SetValueWithoutNotify(v);
            volumeSlider.onValueChanged.AddListener(OnSliderChanged);
        }

        ApplyVolume(v);
        RefreshText(v);
    }

    private void OnDestroy()
    {
        if (volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(OnSliderChanged);
    }

    private void OnSliderChanged(float v)
    {
        ApplyVolume(v);
        PlayerPrefs.SetFloat(PrefKey, v);
        RefreshText(v);
    }

    private void ApplyVolume(float v)
    {
        if (targetAudioSource != null) targetAudioSource.volume = v;
        else AudioListener.volume = v;
    }

    private void RefreshText(float v)
    {
        if (volumeValueText == null) return;
        int pct = Mathf.RoundToInt(v * 100f);
        volumeValueText.text = $"{pct}%";
    }
}
