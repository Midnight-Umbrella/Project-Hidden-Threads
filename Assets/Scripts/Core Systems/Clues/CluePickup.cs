using UnityEngine;
public class CluePickup : MonoBehaviour
{
    [Header("Clue")]
    [SerializeField] private ClueDefinition clue;
    [SerializeField] private string objID;

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.F;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool destroyOnPickup = true;

    [Header("Floating Prompt")]
    [SerializeField] private GameObject floatingFPrompt;

    private bool _inRange;
    private bool _picked;
    public Inventory inventory;

    private void Start()
    {
        if (floatingFPrompt != null)
            floatingFPrompt.SetActive(true);
    }

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
        if (Input.GetKeyUp(interactKey))
        {
            Debug.Log("Pressed");
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
        if (ClueJournal.Instance == null)
        {
            Debug.LogWarning("ClueJournal.Instance is null!");
            return;
        }
        ClueJournal.Instance.AddClue(clue);
        inventory.AddClue(clue);
        _picked = true;
        CluePromptUI.Instance?.Hide();
        CluePopUpUI.Instance?.Show(clue);

        if (floatingFPrompt != null)
            floatingFPrompt.SetActive(false); // hide on pickup

        if (destroyOnPickup) Destroy(gameObject);
        else gameObject.SetActive(false);
    }
    private void HidePrompt()
    {
        if (!_inRange) CluePromptUI.Instance?.Hide();
    }
}