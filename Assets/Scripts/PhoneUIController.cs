using UnityEngine;

public class PhoneUIController : MonoBehaviour
{
    public GameObject phoneUI; // Drag PhoneUI here in Inspector
    private bool isPhoneOpen = false;

    void Update()
    {
        // Press 'P' to toggle phone
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePhone();
        }
    }

    void TogglePhone()
    {
        isPhoneOpen = !isPhoneOpen;
        phoneUI.SetActive(isPhoneOpen);
    }
}