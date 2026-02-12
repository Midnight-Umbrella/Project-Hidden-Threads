using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance {get; private set; }

    [SerializeField] private DialogueUI dialogueUI;

    private string[] currentLines;
    private int currentIndex;
    public bool IsDialogueActive { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        dialogueUI.Hide();
    }

    public void RegisterUI(DialogueUI ui)
    {
        dialogueUI = ui;
    }


    void Update()
    {
        if (!IsDialogueActive) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceDialogue();
        }
    }

    public void StartDialogue(DialogueData data)
    {
        if (IsDialogueActive) return;
        if (data == null || data.lines.Length == 0) return;

        currentLines = data.lines;
        currentIndex = 0;
        IsDialogueActive = true;

        dialogueUI.Show();
        dialogueUI.SetText(currentLines[currentIndex]);
    }

    void AdvanceDialogue()
    {
        currentIndex++;

        if (currentIndex >= currentLines.Length)
        {
            EndDialogue();
        }
        else
        {
            dialogueUI.SetText(currentLines[currentIndex]);
        }
    }

    void EndDialogue()
    {
        IsDialogueActive = false;
        dialogueUI.Hide();
        currentLines = null;
    }
}
