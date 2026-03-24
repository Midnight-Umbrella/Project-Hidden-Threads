using UnityEngine;
using UnityEngine.SceneManagement;

public class PhoneUIController : MonoBehaviour
{
    public GameObject phoneUI; // Drag PhoneUI here in Inspector
    private bool isPhoneOpen = false;
    [SerializeField] ClueDefinition clue1;
    [SerializeField] ClueDefinition clue2;
    [SerializeField] ClueDefinition phoneClue;
    [SerializeField] Inventory inventory;

    void Update()
    {   
        // Press 'P' to toggle phone
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!PhoneCanOpen())
                return;
            Debug.Log("P pressed");
            TogglePhone();
        }
    }

    void TogglePhone()
    {
        isPhoneOpen = !isPhoneOpen;
        phoneUI.SetActive(isPhoneOpen);
    }

    bool PhoneCanOpen()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            if (inventory.Contains(clue1) && inventory.Contains(clue2))
            {
                return true;
            }
            if (inventory.Contains(phoneClue))
                DialogueManager.Instance.StartDialogue("phone", "1");
            return false;
        }
        return true;
    }
}