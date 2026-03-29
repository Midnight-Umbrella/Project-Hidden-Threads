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
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!lockable)
            {
                // Unlocked door, just open
                OpenDoor();
            }
            else if (inventory.Contains(clue))
            {
                // Has the key, open with dialogue
                if (unlockedDialogueNum != null)
                    DialogueManager.Instance.StartDialogue(objID, unlockedDialogueNum);
                OpenDoor();
            }
            else
            {
                // Locked, no key
                if (lockedDialogueNum != null)
                    DialogueManager.Instance.StartDialogue(objID, lockedDialogueNum);
                if (doorAudioSource != null && AudioController.Instance != null)
                    AudioController.Instance.PlaySFXOnSource(doorAudioSource, doorLockedClip, sfxVolume);
                else
                    AudioController.Instance.PlaySFXAtPosition(doorLockedClip, transform.position, sfxVolume);
            }
        }
    }

    private void OpenDoor()
    {
        sr.enabled = false;
        cd.enabled = false;
        if (doorAudioSource != null && AudioController.Instance != null)
            AudioController.Instance.PlaySFXOnSource(doorAudioSource, doorOpenClip, sfxVolume);
        else
            AudioController.Instance.PlaySFXAtPosition(doorOpenClip, transform.position, sfxVolume);
    }
}
