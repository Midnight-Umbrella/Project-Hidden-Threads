using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private ClueDefinition previousClue;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject clue;
    [SerializeField] private DialogueData dialogue;
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
            inventory.AddClue(nonPhysicalClue);
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

            if (dialogue)
            {
                DialogueManager.Instance.StartDialogue(dialogue);
            }
        }
    }
}
