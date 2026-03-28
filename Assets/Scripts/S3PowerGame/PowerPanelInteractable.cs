using UnityEngine;

public class PowerPanelInteractable : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.F;
    [SerializeField] private GameObject interactPrompt;

    private bool playerInRange = false;

    private void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    private void Update()
    {
        if (!playerInRange) return;
        if (Stage3PowerManager.Instance == null) return;
        if (Stage3PowerManager.Instance.PowerRestored) return;

        if (Input.GetKeyDown(interactKey))
        {
            Stage3PowerManager.Instance.OpenMiniGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (interactPrompt != null &&
            Stage3PowerManager.Instance != null &&
            !Stage3PowerManager.Instance.PowerRestored)
        {
            interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }
}