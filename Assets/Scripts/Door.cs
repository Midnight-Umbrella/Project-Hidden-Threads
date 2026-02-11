using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D cd;
    public bool lockable;
    [SerializeField] private ClueDefinition clue;
    public Inventory inventory;
    public AudioSource doorSource;
    public AudioClip doorNoise;
    
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
        if (doorSource != null && doorNoise != null)
        {
            doorSource.clip = doorNoise; //Might change this if we want different door sfx
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !lockable)
        {
            sr.enabled = false;
            cd.enabled = false;
            doorSource.Play();
        }
        else if (collision.gameObject.CompareTag("Player") && inventory.Contains(clue))
        {
            sr.enabled = false;
            cd.enabled = false;
            doorSource.Play();
        }
    }
}
