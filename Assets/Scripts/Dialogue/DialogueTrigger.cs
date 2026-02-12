using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public void TriggerDialogue(DialogueData dialogue)
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
