using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private TextAsset dialogueCSV;

    private string[] dialogue = new string[0];
    private string[] dialogueLine = new string[0];
    private int currentLine = 0;

    public bool IsDialogueActive { get; private set; }

    private void Awake()
    {
        Debug.Log("DialogueManager Awake");

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (dialogueUI != null)
            dialogueUI.Hide();
    }

    private void OnEnable()
    {
        Debug.Log("DialogueManager OnEnable");
    }

    private void Start()
    {
        Debug.Log("DialogueManager Start");
    }

    private void OnDisable()
    {
        Debug.Log("DialogueManager OnDisable");
    }

    private void OnDestroy()
    {
        Debug.Log("DialogueManager destroyed");
    }

    public void RegisterUI(DialogueUI ui)
    {
        dialogueUI = ui;
    }

    private void Update()
    {
        if (!IsDialogueActive) return;
        if (dialogueUI == null) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.F))
        {
            // If the current line is still typing, finish it instantly first.
            if (dialogueUI.IsTyping)
            {
                dialogueUI.FinishTypingInstantly();
            }
            else
            {
                AdvanceDialogue();
            }
        }
    }

    public void StartDialogue(string objID, string dialogueNum)
    {
        if (IsDialogueActive) return;

        if (dialogueCSV == null)
        {
            Debug.LogError("DialogueManager: dialogueCSV is not assigned!");
            return;
        }

        if (dialogueUI == null)
        {
            Debug.LogError("DialogueManager: dialogueUI is not assigned!");
            return;
        }

        string[] lines = dialogueCSV.text.Split('\n');
        dialogueLine = new string[0];
        dialogue = new string[0];
        currentLine = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            string[] line = lines[i].Split(',');

            if (line.Length < 5) continue;

            string csvObjID = line[0].Trim();
            string csvDialogueNum = line[1].Trim();

            if (objID == csvObjID && dialogueNum == csvDialogueNum)
            {
                dialogue = line[4].Trim().Split('|');
                dialogueLine = line;
                break;
            }
        }

        if (dialogue.Length == 0 || dialogueLine.Length == 0)
        {
            Debug.LogWarning($"DialogueManager: No dialogue found for objID={objID}, dialogueNum={dialogueNum}");
            return;
        }

        IsDialogueActive = true;
        dialogueUI.Show();
        DisplayCurrentLine();
    }

    private void AdvanceDialogue()
    {
        currentLine++;

        if (currentLine >= dialogue.Length)
        {
            EndDialogue();
            return;
        }

        DisplayCurrentLine();
    }

    private void DisplayCurrentLine()
    {
        if (dialogueUI == null) return;
        if (dialogue.Length == 0 || currentLine < 0 || currentLine >= dialogue.Length) return;
        if (dialogueLine.Length < 4) return;

        string dialogueType = dialogueLine[3].Trim();
        string currentText = dialogue[currentLine].Trim();

        if (dialogueType == "internal")
        {
            dialogueUI.SetName(dialogueLine[2].Trim());
            dialogueUI.SetText(currentText);
        }
        else if (dialogueType == "conversation")
        {
            // Split only on the first colon, so extra colons in dialogue won't break it.
            int colonIndex = currentText.IndexOf(':');

            if (colonIndex >= 0)
            {
                string speaker = currentText.Substring(0, colonIndex).Trim();
                string lineText = currentText.Substring(colonIndex + 1).Trim();

                dialogueUI.SetName(speaker);
                dialogueUI.SetText(lineText);
            }
            else
            {
                // Fallback if no colon exists
                dialogueUI.SetName(dialogueLine[2].Trim());
                dialogueUI.SetText(currentText);
            }
        }
        else
        {
            // Fallback for unknown dialogue type
            dialogueUI.SetName(dialogueLine[2].Trim());
            dialogueUI.SetText(currentText);
        }
    }

    private void EndDialogue()
    {
        IsDialogueActive = false;

        if (dialogueUI != null)
            dialogueUI.Hide();

        dialogue = new string[0];
        dialogueLine = new string[0];
        currentLine = 0;
    }
}