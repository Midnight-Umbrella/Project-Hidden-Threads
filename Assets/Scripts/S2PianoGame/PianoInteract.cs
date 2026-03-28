using UnityEngine;

public class PianoInteract : MonoBehaviour
{
    [SerializeField] private PianoMinigameUI pianoUI;
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private KeyCode interactKey = KeyCode.F;

    private bool playerInRange = false;

    private void Start()
    {
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (pianoUI == null)
        {
            Debug.Log("PianoInteract: pianoUI is NULL");
            return;
        }

        if (pianoUI.IsOpen) return;

        if (Input.GetKeyDown(interactKey))
        {
            Debug.Log("PianoInteract: F pressed, trying to open piano UI");
            pianoUI.Open();

            if (interactPrompt != null)
            {
                interactPrompt.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("PianoInteract: Player entered range");
        playerInRange = true;

        if (interactPrompt != null)
        {
            interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("PianoInteract: Player left range");
        playerInRange = false;

        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
    }
}