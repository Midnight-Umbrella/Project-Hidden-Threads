using UnityEngine;

public class PhoneUIController : MonoBehaviour
{
    public GameObject phoneUI; // Drag PhoneUI here in Inspector
    private bool isPhoneOpen = false;
    [SerializeField] ClueDefinition clue;
    [SerializeField] Inventory inventory;

    void Update()
    {
        // Press 'P' to toggle phone
        if (Input.GetKeyDown(KeyCode.P) && inventory.Contains(clue))
        {
            Debug.Log("P pressed");
            TogglePhone();
        }
    }

    void TogglePhone()
    {
        isPhoneOpen = !isPhoneOpen;
        phoneUI.SetActive(isPhoneOpen);
    }
}