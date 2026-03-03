using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private DialogueUI dialogueUI;

    private IReadOnlyList<DialogueLine> currentLines;
    private int currentIndex;

    public bool IsDialogueActive { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (dialogueUI != null) dialogueUI.Hide();
    }

    public void RegisterUI(DialogueUI ui)
    {
        dialogueUI = ui;
        if (dialogueUI != null) dialogueUI.Hide();
    }

    private void Update()
    {
        if (!IsDialogueActive) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceDialogue();
        }
    }

    public void StartDialogue(string dialogueId)
    {
        if (IsDialogueActive) return;

        if (dialogueUI == null)
        {
            Debug.LogError("[DialogueManager] dialogueUI is null. Make sure DialogueUI registers itself.");
            return;
        }

        if (DialogueDatabase.Instance == null)
        {
            Debug.LogError("[DialogueManager] DialogueDatabase.Instance is null. Make sure DialogueDatabase exists in the scene.");
            return;
        }

        var lines = DialogueDatabase.Instance.GetDialogue(dialogueId);
        if (lines == null || lines.Count == 0)
        {
            Debug.LogWarning($"[DialogueManager] No dialogue found for id: {dialogueId}");
            return;
        }

        currentLines = lines;
        currentIndex = 0;
        IsDialogueActive = true;

        dialogueUI.Show();
        dialogueUI.SetText(currentLines[currentIndex].Text);
    }

    private void AdvanceDialogue()
    {
        currentIndex++;

        if (currentLines == null || currentIndex >= currentLines.Count)
        {
            EndDialogue();
            return;
        }

        dialogueUI.SetText(currentLines[currentIndex].Text);
    }

    private void EndDialogue()
    {
        IsDialogueActive = false;
        currentLines = null;

        if (dialogueUI != null) dialogueUI.Hide();
    }
}