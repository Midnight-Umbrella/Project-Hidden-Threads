using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.F;

    private Interactable current;

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            Debug.Log($"[PlayerInteract] F pressed. current={(current != null ? current.name : "null")}");

            if (current == null) return;

            if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive) return;

            current.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var interactable = other.GetComponent<Interactable>() ?? other.GetComponentInParent<Interactable>();
        if (interactable == null) return;

        current = interactable;
        Debug.Log($"[PlayerInteract] Enter range: {current.name}");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var interactable = other.GetComponent<Interactable>() ?? other.GetComponentInParent<Interactable>();
        if (interactable == null) return;

        if (current == interactable)
        {
            Debug.Log($"[PlayerInteract] Exit range: {current.name}");
            current = null;
        }
    }
}