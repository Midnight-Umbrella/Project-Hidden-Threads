using UnityEngine;
using TMPro;

public class ConversationListUI : MonoBehaviour
{
    public MessageLoader loader;
    public Transform contentParent;
    public GameObject conversationButtonPrefab;
    public ChatUI chatUI;
    [Header("Locked")]
    public GameObject lockedButton;
    private bool showingLocked = false;
    [SerializeField] private ClueDefinition unlockClue;
    [SerializeField] private Inventory inventory;
    [SerializeField] private ClueDefinition givenClue;
    private bool isLocked;
    private GameObject lockedConvoButton;
    private Conversation lockedConvo;
    private bool first = true;

    void Start()
    {
        lockedButton.gameObject.SetActive(false);
        var conversations = loader.LoadConversations();

        foreach (var convo in conversations)
        {
            GameObject obj = Instantiate(conversationButtonPrefab, contentParent);

            obj.GetComponentInChildren<TextMeshProUGUI>().text = convo.contact;

           
                if (convo.isLocked)
                {   
                    lockedConvo = convo;
                    lockedConvoButton = obj;
                    isLocked = true;
                    obj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                    {
                        ShowLocked();
                    });
                    
                }
                else 
                {
                    obj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                    {
                        chatUI.OpenConversation(convo);
                    });
                }
            
        }
    }

    void Update()
    {
        if (isLocked && inventory.Contains(unlockClue))
        {   
            isLocked = false;
            lockedConvoButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
            lockedConvoButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                chatUI.OpenConversation(lockedConvo);
                if (first) 
                {
                    DialogueManager.Instance.StartDialogue("locked_convo", "0");
                    inventory.AddClue(givenClue);
                    bool first = false;
                }
            });
        }
    }

    private void ShowLocked()
    {
        showingLocked = true;
        lockedButton.SetActive(true);
    }

    public void HideLocked()
    {
        showingLocked = false;
        lockedButton.SetActive(false);
    }
}