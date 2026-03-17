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
    private string[] dialogueLine = new string [0];
    private string[] currentLines;
    private int currentLine = 0;
    public bool IsDialogueActive { get; private set; }

    void Awake()
    {
        Debug.Log("Awake");
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

    void OnEnable()
    {
        Debug.Log("DialogueManager OnEnable");
    }

    void Start()
    {
        Debug.Log("DialogueManager Start");
    }

    void OnDisable()
    {
        Debug.Log("DialogueManager OnDisable");
    }

    void OnDestroy()
    {
        Debug.Log("DialogueManager destroyed");
    }

    public void RegisterUI(DialogueUI ui)
    {
        dialogueUI = ui;
    }


    void Update()
    {
        if (!IsDialogueActive) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.F))
        {
            AdvanceDialogue();
        }
    }

    public void StartDialogue(string objID, string dialogueNum)
    {
        if (IsDialogueActive) return;
        
        string[] lines = dialogueCSV.text.Split('\n');
        dialogueLine = new string [0];
        dialogue = new string [0];
        currentLine = 0;
        
        for (int i = 0; i < lines.Length; i++)
        {
            string[] line = lines[i].Split(',');
            if (line.Length < 5) continue;
            if (objID == line[0] && dialogueNum == line[1])
            {
                dialogue = line[4].Split("|");
                dialogueLine = line;
            }
        }

        IsDialogueActive = true;

        if (dialogue.Length > 0 && dialogueLine.Length > 0) 
        {
            if (dialogueLine[3] == "internal")
            {
                dialogueUI.Show();
                dialogueUI.SetText(dialogue[currentLine]);
                dialogueUI.SetName(dialogueLine[2]);
            }
            else if (dialogueLine[3] == "conversation")
            {
                dialogueUI.Show();
                dialogueUI.SetText(dialogue[currentLine].Split(":")[1]);
                dialogueUI.SetName(dialogue[currentLine].Split(":")[0]);
            }
        }
    }

    void AdvanceDialogue()
    {
        currentLine++;

        if (currentLine >= dialogue.Length)
        {
            EndDialogue();
        }
        else if (dialogueLine[3] == "internal")
        {
            dialogueUI.SetText(dialogue[currentLine]);
        }
        else if (dialogueLine[3] == "conversation")
        {
            string[] nameAndLine = dialogue[currentLine].Split(":");
            dialogueUI.SetText(nameAndLine[1]);
            dialogueUI.SetName(nameAndLine[0]);
            
        }
    }

    void EndDialogue()
    {
        IsDialogueActive = false;
        dialogueUI.Hide();
        currentLines = null;
    }
}
