using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text speakerName;

    [Header("Typewriter Settings")]
    [SerializeField] private float typingSpeed = 0.03f; // seconds per character

    private Coroutine typingCoroutine;
    private string currentFullText = "";
    private bool isTyping = false;

    public bool IsTyping => isTyping;

    private void Awake()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.RegisterUI(this);
        }

        if (dialoguePanel == null)
            Debug.LogError("DialogueUI: dialoguePanel is not assigned!", this);

        if (dialogueText == null)
            Debug.LogError("DialogueUI: dialogueText is not assigned!", this);

        if (speakerName == null)
            Debug.LogError("DialogueUI: speakerName is not assigned!", this);
    }

    public void Show()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);
    }

    public void Hide()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isTyping = false;
        currentFullText = "";

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (dialogueText != null)
            dialogueText.text = "";

        if (speakerName != null)
            speakerName.text = "";
    }

    public void SetText(string text)
    {
        if (dialogueText == null)
        {
            Debug.LogError("DialogueUI: dialogueText is null in SetText()", this);
            return;
        }

        currentFullText = text ?? "";

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeTextCoroutine(currentFullText));
    }

    public void SetName(string name)
    {
        if (speakerName == null)
        {
            Debug.LogError("DialogueUI: speakerName is null in SetName()", this);
            return;
        }

        speakerName.text = (name ?? "") + ":";
    }

    public void FinishTypingInstantly()
    {
        if (!isTyping || dialogueText == null)
            return;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        dialogueText.text = currentFullText;
        isTyping = false;
    }

    private IEnumerator TypeTextCoroutine(string fullText)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in fullText)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }
}