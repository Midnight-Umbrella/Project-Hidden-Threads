using UnityEngine;
using TMPro;

public class ConversationListUI : MonoBehaviour
{
    public MessageLoader loader;
    public Transform contentParent;
    public GameObject conversationButtonPrefab;
    public ChatUI chatUI;

    void Start()
    {
        var conversations = loader.LoadConversations();

        foreach (var convo in conversations)
        {Debug.Log(convo.contact);
            GameObject obj = Instantiate(conversationButtonPrefab, contentParent);

            obj.GetComponentInChildren<TextMeshProUGUI>().text = convo.contact;

            obj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {Debug.Log("ButtonAddedListener");
                chatUI.OpenConversation(convo);
            });
        }
    }
}