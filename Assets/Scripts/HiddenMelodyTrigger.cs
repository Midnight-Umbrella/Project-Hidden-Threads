using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenMelodyTrigger : MonoBehaviour
{
    public GameObject hiddenMelodyPopUp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hiddenMelodyPopUp.SetActive(true);
        }
    }
}
