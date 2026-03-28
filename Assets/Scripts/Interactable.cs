using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("System:")]
    [SerializeField] private string objID; 
    [SerializeField] private Inventory inventory;
    [Header("Required Clue (if applicable):")]
    [SerializeField] private ClueDefinition previousClue;
    [SerializeField] private string preDialogueNum;
    [Header("Given Clue (if applicable)")]
    [SerializeField] private GameObject clue;
    [SerializeField] private string dialogueNum;
    [Header("Non Physical Clue (if applicable)")]
    [SerializeField] private bool isNotPhysicalClue;
    [SerializeField] private ClueDefinition nonPhysicalClue;
    [Header("Audio")]
    [SerializeField] private AudioClip inspectClip;
    [SerializeField] private float inspectVolume = 1f;
    private AudioSource objectAudioSource;

    [Header("Clue Options")]
    [SerializeField] private bool isClue;
    [SerializeField] private bool delayCluePrompt;
    private Collider2D col;
    private SpriteRenderer sr;

    void Awake()
    {
        if (clue && !isNotPhysicalClue) 
        {
            col = clue.GetComponent<Collider2D>();
            sr = clue.GetComponent<SpriteRenderer>();
        }
    }
    
    public void Interact()
    {
        if (isClue)
        {
            CluePickup cp = gameObject.GetComponent<CluePickup>();
            cp.TryPickup();
            return;
        }

        if (isNotPhysicalClue)
        {
            if (!previousClue || inventory.Contains(previousClue)) 
            {
                if (delayCluePrompt)
                {
                    DialogueManager.Instance.StartDialogue(objID, dialogueNum);
                    inventory.AddClue(nonPhysicalClue);
                    return;
                }
                inventory.AddClue(nonPhysicalClue);
                DialogueManager.Instance.StartDialogue(objID, dialogueNum);
            }
            else if(preDialogueNum != null)
            {
                DialogueManager.Instance.StartDialogue(objID, preDialogueNum);
            }
                
        }
        else if (!previousClue || inventory.Contains(previousClue)) 
        {
            if (col)
            {
                col.enabled = true;
            }

            if (sr)
            {
                sr.enabled = true;
            }

            if (dialogueNum != null)
            {
                if (objectAudioSource != null)
                    AudioController.Instance.PlaySFXOnSource(objectAudioSource, inspectClip, inspectVolume);
                //else
                    //AudioController.Instance.PlaySFXAtPosition(inspectClip, transform.position, inspectVolume);
                DialogueManager.Instance.StartDialogue(objID,dialogueNum);
            }
        }
        else if(preDialogueNum != null)
        {
            if (objectAudioSource != null)
                    AudioController.Instance.PlaySFXOnSource(objectAudioSource, inspectClip, inspectVolume);
                else
                    AudioController.Instance.PlaySFXAtPosition(inspectClip, transform.position, inspectVolume);
            DialogueManager.Instance.StartDialogue(objID, preDialogueNum);
        }
    }
}