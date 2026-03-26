using UnityEngine;
using TMPro;

public class SignalTarget : MonoBehaviour
{
    [Header("Target Info")]
    [SerializeField] private string targetName = "Hidden Item";
    [SerializeField] [TextArea] private string foundMessage = "You found the hidden item.";
    [SerializeField] private float interactDistance = 1.2f;
    [SerializeField] private KeyCode interactKey = KeyCode.F;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private SignalTrackerController tracker;

    [Header("Optional UI")]
    [SerializeField] private GameObject promptUI;
    [SerializeField] private TMP_Text promptText;

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
            promptText.text = $"Press {interactKey} to collect";

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

        if (tracker != null)
            tracker.StopTracking();

        Debug.Log(foundMessage);

        // Add your clue / inventory logic here
        // Example:
        // ClueJournal.Instance.AddClue(clueDefinition);
        // DialogueManager.Instance.StartDialogue("found_item");
    }
}