using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string objID; 
    [SerializeField] private ClueDefinition previousClue;
    [SerializeField] private string preDialogueNum;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject clue;
    [SerializeField] private string dialogueNum;
    [SerializeField] private bool isNotPhysicalClue;
    [SerializeField] private ClueDefinition nonPhysicalClue;
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
        if (isNotPhysicalClue)
        {
            if (!previousClue || inventory.Contains(previousClue)) 
            {
                inventory.AddClue(nonPhysicalClue);

                if (dialogueNum != null)
                {
                    DialogueManager.Instance.StartDialogue(objID,dialogueNum);
                }
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
                DialogueManager.Instance.StartDialogue(objID,dialogueNum);
            }
        }
        else if(preDialogueNum != null)
        {
            DialogueManager.Instance.StartDialogue(objID, preDialogueNum);
        }
    }
}
