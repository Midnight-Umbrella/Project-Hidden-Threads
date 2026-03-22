using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro support

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text speakerName;

    void Awake()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.RegisterUI(this);
        }
    }

    public void Show()
    {
        dialoguePanel.SetActive(true);
    }

    public void Hide()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    public void SetText(string text)
    {
        dialogueText.text = text;
    }

    public void SetName(string name)
    {
        speakerName.text = name + ":";
    }
}
