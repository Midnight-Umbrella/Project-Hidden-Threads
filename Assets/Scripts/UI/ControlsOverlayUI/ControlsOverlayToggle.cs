using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class ControlsOverlayToggle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject contentPanel;
    [SerializeField] private Button toggleButton;
    [SerializeField] private TextMeshProUGUI toggleButtonText; 

    [Header("Settings")]
    [SerializeField] private string showLabel = "\u25B2 Controls";
    [SerializeField] private string hideLabel = "\u25BC Controls";
    [SerializeField] private bool startCollapsed = false;

    private bool isCollapsed;

    private void Start()
    {
        isCollapsed = startCollapsed;
        ApplyState();
        toggleButton.onClick.AddListener(Toggle);
    }

    public void Toggle()
    {
        isCollapsed = !isCollapsed;
        ApplyState();
    }

    private void ApplyState()
    {
        contentPanel.SetActive(!isCollapsed);
        toggleButtonText.text = isCollapsed ? showLabel : hideLabel;
    }
}