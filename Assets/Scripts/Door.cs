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
    
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !lockable)
        {
            sr.enabled = false;
            cd.enabled = false;
        }
        else if (collision.gameObject.CompareTag("Player") && inventory.Contains(clue))
        {
            sr.enabled = false;
            cd.enabled = false;
        }
    }
}
