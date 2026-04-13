using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private List<ClueDefinition> additionalClues;
    [Header("Audio")]
    [SerializeField] private AudioClip inspectClip;
    [SerializeField] private float inspectVolume = 1f;
    private AudioSource objectAudioSource;

    [Header("Clue Options")]
    [SerializeField] private bool isClue;
    [SerializeField] private bool delayCluePrompt;
    private Collider2D col;
    private SpriteRenderer sr;
    [Header("Event Trigger")]
    [SerializeField] private bool isEventTrigger;
    [SerializeField] private UnityEvent triggeredEvent;
    private bool isFinalDialogue = false;

    void Awake()
    {
        if (clue && !isNotPhysicalClue) 
        {
            col = clue.GetComponent<Collider2D>();
            sr = clue.GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        if (isFinalDialogue && !DialogueManager.Instance.IsDialogueActive)
        {
            triggeredEvent?.Invoke();
            return;
        }
    }
    
    public void Interact()
    {
        if (isEventTrigger)
        {
            DialogueManager.Instance.StartDialogue(objID, dialogueNum);
            isFinalDialogue = true;
            return;
        }


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
                    foreach (var c in additionalClues)
                        inventory.AddClue(c);
                    return;
                }
                inventory.AddClue(nonPhysicalClue);
                foreach (var c in additionalClues)
                    inventory.AddClue(c);
                DialogueManager.Instance.StartDialogue(objID, dialogueNum);
            }
            else if (preDialogueNum != null)
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
                if (objectAudioSource != null && AudioController.Instance != null)
                    AudioController.Instance.PlaySFXOnSource(objectAudioSource, inspectClip, inspectVolume);
                else if (AudioController.Instance != null)
                    AudioController.Instance.PlaySFXAtPosition(inspectClip, transform.position, inspectVolume);
                DialogueManager.Instance.StartDialogue(objID,dialogueNum);
            }
        }
        else if(preDialogueNum != null)
        {
            if (objectAudioSource != null && AudioController.Instance != null)
                    AudioController.Instance.PlaySFXOnSource(objectAudioSource, inspectClip, inspectVolume);
                else if (AudioController.Instance != null)
                    AudioController.Instance.PlaySFXAtPosition(inspectClip, transform.position, inspectVolume);
            DialogueManager.Instance.StartDialogue(objID, preDialogueNum);
        }

        
    }
    public AudioClip GetInspectClip()
    {
        return inspectClip;
    }
}