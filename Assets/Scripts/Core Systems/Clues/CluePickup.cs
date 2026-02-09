using UnityEngine;

public class CluePickup : MonoBehaviour
{
    [Header("Clue")]
    [SerializeField] private ClueDefinition clue;

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.F;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool destroyOnPickup = true;

    private bool _inRange;
    private bool _picked;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_picked) return;
        if (!other.CompareTag(playerTag)) return;

        _inRange = true;
        CluePromptUI.Instance?.Show($"Press {interactKey} to collect: {(clue != null ? clue.title : "clue")}");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        _inRange = false;
        CluePromptUI.Instance?.Hide();
    }

    private void Update()
    {
        if (_picked) return;
        if (!_inRange) return;

        if (Input.GetKeyDown(interactKey))
        {
            TryPickup();
        }
    }

    private void TryPickup()
    {
        if (clue == null)
        {
            Debug.LogWarning($"{name}: clue reference is missing.");
            return;
        }

        var journal = ClueJournal.Instance;
        if (journal == null)
        {
            Debug.LogError("No ClueJournal in scene. Create a CoreSystems object with ClueJournal.");
            return;
        }

        bool added = journal.AddClue(clue);
        if (added)
        {
            _picked = true;
            CluePromptUI.Instance?.Show($"Collected: {clue.title}");
            Invoke(nameof(HidePrompt), 0.8f);

            if (destroyOnPickup) Destroy(gameObject);
            else gameObject.SetActive(false);
        }
        else
        {
            CluePromptUI.Instance?.Show($"Already collected: {clue.title}");
            Invoke(nameof(HidePrompt), 0.8f);
        }
    }

    private void HidePrompt()
    {
        if (!_inRange) CluePromptUI.Instance?.Hide();
    }
}
