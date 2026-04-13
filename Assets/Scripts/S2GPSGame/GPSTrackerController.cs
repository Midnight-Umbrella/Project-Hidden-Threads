using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GPSTrackerController : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject trackerPanel;
    [SerializeField] private RectTransform arrowUI;

    [Header("UI References")]
    [SerializeField] private TMP_Text targetNameText;
    [SerializeField] private TMP_Text signalText;
    [SerializeField] private Slider signalSlider;

    [Header("Tracking Settings")]
    [SerializeField] private float maxDetectDistance = 12f;
    [SerializeField] private float strongDistance = 5f;
    [SerializeField] private float veryStrongDistance = 1.5f;

    private GPSClueTarget currentTarget;
    private bool trackingActive;

    private void Awake()
    {
        if (trackerPanel != null)
            trackerPanel.SetActive(false);
    }

    private void Update()
    {
        if (!trackingActive) return;
        if (player == null || currentTarget == null) return;

        if (currentTarget.IsCollected)
        {
            StopTracking();
            return;
        }

        UpdateTrackerUI();
    }

    private void UpdateTrackerUI()
    {
        Vector2 dir = currentTarget.transform.position - player.position;
        float distance = dir.magnitude;

        if (arrowUI != null)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arrowUI.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        if (targetNameText != null)
            targetNameText.text = currentTarget.TargetName;

        float normalized = Mathf.Clamp01(1f - (distance / maxDetectDistance));

        if (signalSlider != null)
            signalSlider.value = normalized;

        if (signalText != null)
        {
            if (distance > maxDetectDistance)
                signalText.text = "No Signal";
            else if (distance > strongDistance)
                signalText.text = "Weak";
            else if (distance > veryStrongDistance)
                signalText.text = "Strong";
            else
                signalText.text = "Very Strong";
        }
    }

    public void StartTracking(GPSClueTarget target)
    {
        if (target == null) return;

        currentTarget = target;
        trackingActive = true;

        if (trackerPanel != null)
            trackerPanel.SetActive(true);

        UpdateTrackerUI();
    }

    public void StopTracking()
    {
        trackingActive = false;

        if (trackerPanel != null)
            trackerPanel.SetActive(false);
    }

    public GPSClueTarget GetCurrentTarget()
    {
        return currentTarget;
    }

    public GameObject GetTrackerPanel()
    {
        return trackerPanel;
    }

    public bool IsTrackingActive()
    {
        return trackingActive;
    }
}