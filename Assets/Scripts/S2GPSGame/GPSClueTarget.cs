using UnityEngine;
using TMPro;

public class GPSClueTarget : MonoBehaviour
{
    [Header("Target Info")]
    [SerializeField] private string targetName = "Hidden Clue";
    [SerializeField] [TextArea] private string foundMessage = "You found a hidden clue.";
    [SerializeField] private string hiddenMelodyCode = "31562";
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private KeyCode interactKey = KeyCode.F;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GPSTrackerController tracker;

    [Header("Optional UI")]
    [SerializeField] private GameObject promptUI;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private GPSCluePopupUI popupUI;

    [Header("Optional")]
    [SerializeField] private GameObject objectToHideAfterPickup;

    public string TargetName => targetName;
    public bool IsCollected { get; private set; }

    private void Awake()
    {
        if (promptUI != null)
            promptUI.SetActive(false);

        if (objectToHideAfterPickup == null)
            objectToHideAfterPickup = gameObject;
    }

    private void Update()
    {
        if (IsCollected) return;
        if (player == null || tracker == null) return;

        bool isCurrentTarget = tracker.GetCurrentTarget() == this;
        float distance = Vector2.Distance(player.position, transform.position);
        bool canCollect = isCurrentTarget && distance <= interactDistance;

        if (promptUI != null)
            promptUI.SetActive(canCollect);

        if (promptText != null && canCollect)
            promptText.text = $"Press {interactKey} to collect clue";

        if (canCollect && Input.GetKeyDown(interactKey))
        {
            Collect();
        }
    }

    public void Collect()
    {
        if (IsCollected) return;

        IsCollected = true;

        if (promptUI != null)
            promptUI.SetActive(false);

        if (objectToHideAfterPickup != null)
            objectToHideAfterPickup.SetActive(false);

        if (GPSMelodyClueManager.Instance != null)
        {
            GPSMelodyClueManager.Instance.SetGPSClue(targetName, hiddenMelodyCode, foundMessage);
        }

        if (popupUI != null)
        {
            popupUI.ShowClue(targetName, hiddenMelodyCode, foundMessage);
        }

        if (tracker != null)
            tracker.StopTracking();

        Debug.Log($"{targetName} collected. Melody code = {hiddenMelodyCode}");
    }
}