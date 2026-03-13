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
