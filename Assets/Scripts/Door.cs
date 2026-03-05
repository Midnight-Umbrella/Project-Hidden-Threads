using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D cd;
    private AudioSource doorAudioSource;
    public bool lockable;
    [SerializeField] private ClueDefinition clue;
    [SerializeField] private string objID;
    [SerializeField] private string lockedDialogueNum;
    [SerializeField] private string unlockedDialogueNum;
    public Inventory inventory;

    [Header("Audio")]
    [SerializeField] private AudioClip doorOpenClip;
    [SerializeField] private AudioClip doorLockedClip;
    [SerializeField] private float sfxVolume = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
        doorAudioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !lockable)
        {
            sr.enabled = false;
            cd.enabled = false;
            // Play door opening sound from door's AudioSource
            if (doorAudioSource != null)
                AudioController.Instance.PlaySFXOnSource(doorAudioSource, doorOpenClip, sfxVolume);
            else
                AudioController.Instance.PlaySFXAtPosition(doorOpenClip, transform.position, sfxVolume);
        }
        else if (collision.gameObject.CompareTag("Player") && inventory.Contains(clue))
        {   
            if (unlockedDialogueNum != null)
            {
                DialogueManager.Instance.StartDialogue(objID, unlockedDialogueNum);
            }

            sr.enabled = false;
            cd.enabled = false;
            // Play door opening sound from door's AudioSource
            if (doorAudioSource != null)
                AudioController.Instance.PlaySFXOnSource(doorAudioSource, doorOpenClip, sfxVolume);
            else
                AudioController.Instance.PlaySFXAtPosition(doorOpenClip, transform.position, sfxVolume);
        }
        else if (lockable && !inventory.Contains(clue) && (lockedDialogueNum != null))
        {
            DialogueManager.Instance.StartDialogue(objID, lockedDialogueNum);
        }
    }
}
