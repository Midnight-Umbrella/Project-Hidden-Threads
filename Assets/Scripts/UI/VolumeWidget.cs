using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeWidget : MonoBehaviour
{
    [Header("UI Refs")]
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text valueText; // 可空

    private bool _ignore;

    private void Awake()
    {
        // Make sure Volume Initial Volume Accurate 
        VolumeBus.ApplySaved();

        if (slider == null) slider = GetComponentInChildren<Slider>();

        // Initialize UI Display 
        SetUI(VolumeBus.Get());

        // 监听UI变化 -> 写入全局
        if (slider != null)
            slider.onValueChanged.AddListener(OnSliderChanged);

        // Change the entire environment -> Update UI (Use “Two-side view/Two-side UI same step”)
        VolumeBus.OnChanged += SetUI;
    }

    private void OnDestroy()
    {
        VolumeBus.OnChanged -= SetUI;
    }

    private void OnSliderChanged(float v)
    {
        if (_ignore) return;
        VolumeBus.Set(v);
    }

    private void SetUI(float v)
    {
        _ignore = true;
        if (slider != null) slider.SetValueWithoutNotify(v);
        if (valueText != null) valueText.text = $"{Mathf.RoundToInt(v * 100f)}%";
        _ignore = false;
    }
}
