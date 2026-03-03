public class ContactButton : MonoBehaviour
{
    public string contactName;      // Name of contact (Louise, Roomie, etc.)
    public GameObject messagePanel;  // The panel that shows the conversation
    public TMP_Text headerText;      // The text at the top that shows who you're texting

    void Start()
    {
        // When the game starts, runs the OpenConversation function"
        GetComponent<Button>().onClick.AddListener(OpenConversation);
    }

    void OpenConversation()
    {
        // Hide the contact list
        transform.parent.gameObject.SetActive(false);

        // Show the message panel
        if (messagePanel != null)
            messagePanel.SetActive(true);

        // Change the header to show who we're talking to
        if (headerText != null)
            headerText.text = contactName;
    }
}