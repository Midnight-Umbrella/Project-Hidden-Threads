using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Dialogue (CSV id)")]
    [SerializeField] private string dialogueId;

    // 给 PlayerInteract 调用
    public void Interact()
    {
        if (!string.IsNullOrEmpty(dialogueId))
        {
            DialogueManager.Instance.StartDialogue(dialogueId);
        }
        else
        {
            Debug.LogWarning($"Interactable '{name}' has no dialogueId set.");
        }
    }
}