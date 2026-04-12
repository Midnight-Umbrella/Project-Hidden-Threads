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

    [Header("Audio")]
    [SerializeField] private AudioSource beepSource;
    [SerializeField] private AudioClip beepClip;

    [SerializeField] private float maxBeepInterval = 1.0f; // far = slow
    [SerializeField] private float minBeepInterval = 0.1f; // close = fast

    [SerializeField] private float minPitch = 0.8f;
    [SerializeField] private float maxPitch = 2.0f;

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

        if (beepSource != null && beepClip != null)
        {
            if (beepRoutine != null)
                StopCoroutine(beepRoutine);
            beepRoutine = StartCoroutine(BeepLoop());
        }
    }

    public void StopTracking()
    {
        trackingActive = false;

        if (trackerPanel != null)
            trackerPanel.SetActive(false);
        
        if (beepRoutine != null)
            StopCoroutine(beepRoutine);
        beepRoutine = null;
    }

    public GPSClueTarget GetCurrentTarget()
    {
        return currentTarget;
    }

    public bool IsTrackingActive()
    {
        return trackingActive;
    }

    private IEnumerator BeepLoop()
    {
        while (trackingActive && currentTarget != null)
        {
            float distance = Vector3.Distance(player.position, currentTarget.transform.position);

            float t = Mathf.Clamp01(1f - (distance / maxDetectDistance));

            float interval = Mathf.Lerp(maxBeepInterval, minBeepInterval, t);
            float pitch = Mathf.Lerp(minPitch, maxPitch, t);

            if (beepSource != null && beepClip != null)
            {
                beepSource.pitch = Mathf.Lerp(beepSource.pitch, pitch, 0.1f);
                beepSource.PlayOneShot(beepClip);
            }

            yield return new WaitForSeconds(interval);
        }
    }
}