using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public void TriggerDialogue(string objID, string dialogueNum)
    {
        DialogueManager.Instance.StartDialogue(objID, dialogueNum);
    }
}
