using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AddClueFromPost : MonoBehaviour
{
    [SerializeField] ClueDefinition clue;
    [SerializeField] Inventory inventory;
    private bool first = true;
    
    public void GiveClue()
    {
        if (first && SceneManager.GetActiveScene().name == "GameScene")
        {
            first = false;
            inventory.AddClue(clue);
            DialogueManager.Instance.StartDialogue("location", "0");
        }
    }
}
