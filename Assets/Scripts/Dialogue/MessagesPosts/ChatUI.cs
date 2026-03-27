using UnityEngine;
using TMPro;

public class ChatUI : MonoBehaviour
{
    public Transform contentParent;
    public GameObject playerMessagePrefab;
    public GameObject contactMessagePrefab;
    public TextMeshProUGUI headerText;

    public void OpenConversation(Conversation convo)
    {
        headerText.text = convo.contact;

        // Clear old messages
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var msg in convo.messages)
        {
            GameObject prefab = (msg.sender == "Vera") 
                ? playerMessagePrefab 
                : contactMessagePrefab;

            GameObject bubble = Instantiate(prefab, contentParent);

            bubble.GetComponentInChildren<TextMeshProUGUI>().text = msg.text;
        }

        gameObject.SetActive(true);
    }
}