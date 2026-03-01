using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private string dialogueId;

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogueId);
    }
}