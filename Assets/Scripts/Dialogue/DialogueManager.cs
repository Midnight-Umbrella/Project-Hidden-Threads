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
        if (DialogueDatabase.Instance == null)
        {
            Debug.LogError("DialogueDatabase.Instance is null. Make sure DialogueDatabase exists in the scene.");
            return;
        }

        var lines = DialogueDatabase.Instance.GetDialogue(dialogueId);
        if (lines == null || lines.Count == 0) return;

        currentLines = lines;
        currentIndex = 0;
        IsDialogueActive = true;

        dialogueUI.Show();
        dialogueUI.SetText(currentLines[currentIndex].text);
    }

    private void AdvanceDialogue()
    {
        currentIndex++;

        if (currentLines == null || currentIndex >= currentLines.Count)
        {
            EndDialogue();
            return;
        }

        dialogueUI.SetText(currentLines[currentIndex].text);
    }

    private void EndDialogue()
    {
        IsDialogueActive = false;
        currentLines = null;

        dialogueUI.Hide();
    }
}