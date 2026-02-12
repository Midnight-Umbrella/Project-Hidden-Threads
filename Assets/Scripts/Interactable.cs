using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject clue;
    private Collider2D col;
    private SpriteRenderer sr;

    void Awake()
    {
        col = clue.GetComponent<Collider2D>();
        sr = clue.GetComponent<SpriteRenderer>();
    }
    
    public void Interact()
    {
        if (col != null)
        {
            col.enabled = true;
        }

        if (sr != null)
        {
            sr.enabled = true;
        }
    }
}
