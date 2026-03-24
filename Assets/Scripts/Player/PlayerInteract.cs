using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange;
    public LayerMask interactLayer;

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetKeyUp(KeyCode.F))
        {
            if (DialogueManager.Instance.ignoreNextKeyPress)
            {
                DialogueManager.Instance.ignoreNextKeyPress = false;
                return;
            }
            if (DialogueManager.Instance.IsDialogueActive) return;
            Debug.Log("Trying Interact");
            TryInteract();
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            if (DialogueManager.Instance.ignoreNextKeyPress)
            {
                DialogueManager.Instance.ignoreNextKeyPress = false;
                return;
            }
        }
    }

    void TryInteract()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            transform.position,
            interactRange,
            interactLayer
        );
        
        if (hit != null)
        {
            Interactable interactable = hit.GetComponent<Interactable>();
            if (interactable != null)
            {Debug.Log("Interacting");
                interactable.Interact();
            }
        }
    }
}
