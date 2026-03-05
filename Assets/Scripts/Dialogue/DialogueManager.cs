using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance {get; private set; }

    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private TextAsset dialogueCSV;
    private string[] dialogue = new string [0];
    private string[] currentLines;
    private int currentLine = 0;
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

    public void StartDialogue(string objID, string dialogueNum)
    {
    Debug.Log(objID);
    Debug.Log(dialogueNum);
        if (IsDialogueActive) return;
        string[] lines = dialogueCSV.text.Split('\n');
        dialogue = null;
        currentLine = 0;
        
        for (int i = 0; i < lines.Length; i++)
        {
            string[] line = lines[i].Split(',');
            if (line.Length < 5) continue;
            if (objID == line[0] && dialogueNum == line[1])
            {
                dialogue = line[4].Split("|");
            }
        }

        IsDialogueActive = true;

        if (dialogue.Length > 0) 
        {
            dialogueUI.Show();
            dialogueUI.SetText(dialogue[currentLine]);
        }
    }

    void AdvanceDialogue()
    {
        currentLine++;

        if (currentLine >= dialogue.Length)
        {
            EndDialogue();
        }
        else
        {
            dialogueUI.SetText(dialogue[currentLine]);
        }
    }

    void EndDialogue()
    {
        IsDialogueActive = false;
        dialogueUI.Hide();
        currentLines = null;
    }
}
