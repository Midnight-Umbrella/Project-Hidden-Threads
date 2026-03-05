using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDialogueTrigger : MonoBehaviour
{
    [SerializeField] private string objID;
    [SerializeField] private string dialogueNum;

    public void OnTriggerEnter2D(Collider2D other)
    {
        DialogueManager.Instance.StartDialogue(objID, dialogueNum);
        gameObject.SetActive(false);
    }
}
