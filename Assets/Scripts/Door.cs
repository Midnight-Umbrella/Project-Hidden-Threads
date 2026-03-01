using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D cd;

    public bool lockable;

    [SerializeField] private ClueDefinition clue;
    [SerializeField] private string dialogueId;   // CSV dialogue id

    public Inventory inventory;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // Door not lockable → open
        if (!lockable)
        {
            OpenDoor();
            return;
        }

        // Has required clue → open
        if (inventory != null && clue != null && inventory.Contains(clue))
        {
            OpenDoor();
            return;
        }

        // Locked + missing clue → show dialogue
        if (!string.IsNullOrEmpty(dialogueId))
        {
            DialogueManager.Instance.StartDialogue(dialogueId);
        }
    }

    private void OpenDoor()
    {
        if (sr != null) sr.enabled = false;
        if (cd != null) cd.enabled = false;
    }
}