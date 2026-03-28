using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PianoMinigameUI : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool enableDebugLogs = true;

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Button[] noteButtons;
    [SerializeField] private TMP_Text hintText;
    [SerializeField] private TMP_Text playedText;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] noteClips;

    [Header("Keyboard")]
    [SerializeField] private KeyCode[] keyInputs = new KeyCode[]
    {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8
    };

    [Header("Close Keys")]
    [SerializeField] private KeyCode closeKey = KeyCode.Escape;
    [SerializeField] private KeyCode altCloseKey = KeyCode.F;

    [Header("Optional")]
    [SerializeField] private Behaviour playerMovementScript;

    [Header("Melody Puzzle (Optional)")]
    [SerializeField] private bool useTargetMelody = false;
    [SerializeField] private int[] targetMelody;
    [SerializeField] private UnityEvent onSolved;

    private readonly string[] noteNames =
    {
        "Do", "Re", "Mi", "Fa", "So", "La", "Ti", "Do"
    };

    private readonly List<int> playedNotes = new List<int>();
    private bool isOpen = false;
    private bool isSolved = false;

    public bool IsOpen => isOpen;

    private void Awake()
    {
        Log("Awake() called.");
        ValidateReferences("Awake");

        if (panel != null)
        {
            panel.SetActive(false);
            Log("Panel was set inactive in Awake().");
        }
        else
        {
            LogWarning("Panel is NULL in Awake(). Piano UI cannot open.");
        }
    }

    private void Start()
    {
        Log("Start() called.");
        SetupButtons();
        UpdateHint("Click the keys or press 1-8.");
        UpdatePlayedText();
        ValidateReferences("Start");
    }

    private void Update()
    {
        if (!isOpen) return;

        if (keyInputs == null)
        {
            LogWarning("keyInputs is NULL.");
            return;
        }

        if (noteClips == null)
        {
            LogWarning("noteClips is NULL.");
            return;
        }

        for (int i = 0; i < keyInputs.Length && i < noteClips.Length; i++)
        {
            if (Input.GetKeyDown(keyInputs[i]))
            {
                Log($"Keyboard input detected: {keyInputs[i]} -> note index {i} ({GetNoteName(i)})");
                PlayNote(i);
            }
        }

        if (Input.GetKeyDown(closeKey) || Input.GetKeyDown(altCloseKey))
        {
            Log($"Close key pressed. closeKey={closeKey}, altCloseKey={altCloseKey}");
            Close();
        }
    }

    private void SetupButtons()
    {
        if (noteButtons == null)
        {
            LogWarning("noteButtons array is NULL in SetupButtons().");
            return;
        }

        Log($"SetupButtons() called. noteButtons length = {noteButtons.Length}");

        for (int i = 0; i < noteButtons.Length; i++)
        {
            int index = i;

            if (noteButtons[i] == null)
            {
                LogWarning($"noteButtons[{i}] is NULL.");
                continue;
            }

            noteButtons[i].onClick.RemoveAllListeners();
            noteButtons[i].onClick.AddListener(() => PlayNote(index));

            Log($"Bound button [{i}] '{noteButtons[i].name}' to note index {index} ({GetNoteName(index)}).");
        }
    }

    public void Open()
    {
        Log("Open() called.");

        isOpen = true;
        isSolved = false;
        playedNotes.Clear();

        if (panel != null)
        {
            panel.SetActive(true);
            Log($"Panel '{panel.name}' activated successfully.");
        }
        else
        {
            LogError("Panel is NULL in Open(). Cannot show piano UI.");
        }

        if (playerMovementScript != null)
        {
            Log($"Disabling player movement script: {playerMovementScript.GetType().Name}");
            playerMovementScript.enabled = false;
        }
        else
        {
            LogWarning("playerMovementScript is NULL in Open(). Player will still be able to move.");
        }

        if (useTargetMelody && targetMelody != null && targetMelody.Length > 0)
        {
            UpdateHint("Play the correct melody.");
            Log($"Melody mode ON. Target melody length = {targetMelody.Length}");
        }
        else
        {
            UpdateHint("Click the keys or press 1-8.");
            Log("Melody mode OFF. Free play mode.");
        }

        UpdatePlayedText();
        ValidateReferences("Open");
    }

    public void Close()
    {
        Log("Close() called.");

        isOpen = false;

        if (panel != null)
        {
            panel.SetActive(false);
            Log($"Panel '{panel.name}' deactivated successfully.");
        }
        else
        {
            LogWarning("Panel is NULL in Close().");
        }

        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
            Log($"Re-enabled player movement script: {playerMovementScript.GetType().Name}");
        }
        else
        {
            LogWarning("playerMovementScript is NULL in Close().");
        }
    }

    public void ClearPlayedNotes()
    {
        Log("ClearPlayedNotes() called.");
        playedNotes.Clear();
        UpdateHint("Sequence cleared.");
        UpdatePlayedText();
    }

    private void PlayNote(int index)
    {
        Log($"PlayNote({index}) called. isOpen={isOpen}");

        if (!isOpen)
        {
            LogWarning("PlayNote() ignored because piano UI is not open.");
            return;
        }

        if (noteClips == null)
        {
            LogError("noteClips array is NULL.");
            return;
        }

        if (index < 0 || index >= noteClips.Length)
        {
            LogError($"PlayNote() index out of range. index={index}, noteClips.Length={noteClips.Length}");
            return;
        }

        AudioClip clip = noteClips[index];

        if (audioSource == null)
        {
            LogError("audioSource is NULL. No sound will play.");
        }
        else if (clip == null)
        {
            LogError($"noteClips[{index}] is NULL. No sound assigned for {GetNoteName(index)}.");
        }
        else
        {
            audioSource.PlayOneShot(clip);
            Log($"Played note: index={index}, name={GetNoteName(index)}, clip={clip.name}");
        }

        playedNotes.Add(index);
        Log($"Current played count = {playedNotes.Count}");
        UpdatePlayedText();

        if (useTargetMelody)
        {
            CheckMelody();
        }
    }

    private void CheckMelody()
    {
        Log("CheckMelody() called.");

        if (isSolved)
        {
            Log("Melody already solved. Check skipped.");
            return;
        }

        if (targetMelody == null || targetMelody.Length == 0)
        {
            LogWarning("Target melody is NULL or empty.");
            return;
        }

        int currentIndex = playedNotes.Count - 1;

        if (currentIndex < 0)
        {
            LogWarning("CheckMelody() called but no notes have been played.");
            return;
        }

        Log($"Comparing playedNotes[{currentIndex}]={playedNotes[currentIndex]} with targetMelody[{currentIndex}]={targetMelody[currentIndex]}");

        if (currentIndex >= targetMelody.Length)
        {
            LogWarning("Played sequence is longer than target melody. Resetting.");
            playedNotes.Clear();
            UpdateHint("Wrong melody. Try again.");
            UpdatePlayedText();
            return;
        }

        if (playedNotes[currentIndex] != targetMelody[currentIndex])
        {
            LogWarning($"Wrong note at position {currentIndex}. Expected {GetNoteName(targetMelody[currentIndex])}, got {GetNoteName(playedNotes[currentIndex])}. Resetting.");
            playedNotes.Clear();
            UpdateHint("Wrong melody. Try again.");
            UpdatePlayedText();
            return;
        }

        if (playedNotes.Count == targetMelody.Length)
        {
            isSolved = true;
            UpdateHint("Success!");
            Log("Target melody solved successfully. Invoking onSolved event.");
            onSolved?.Invoke();
        }
        else
        {
            Log($"Melody progress OK: {playedNotes.Count}/{targetMelody.Length}");
        }
    }

    private void UpdateHint(string message)
    {
        if (hintText != null)
        {
            hintText.text = message;
            Log($"Hint updated: {message}");
        }
        else
        {
            LogWarning($"hintText is NULL. Cannot show hint: {message}");
        }
    }

    private void UpdatePlayedText()
    {
        if (playedText == null)
        {
            LogWarning("playedText is NULL. Played sequence will not be displayed.");
            return;
        }

        if (playedNotes.Count == 0)
        {
            playedText.text = "Played: (none)";
            Log("Played text updated: Played: (none)");
            return;
        }

        string result = "Played: ";

        for (int i = 0; i < playedNotes.Count; i++)
        {
            int index = playedNotes[i];

            if (index >= 0 && index < noteNames.Length)
            {
                result += noteNames[index];
            }
            else
            {
                result += "?";
            }

            if (i < playedNotes.Count - 1)
            {
                result += " ";
            }
        }

        playedText.text = result;
        Log($"Played text updated: {result}");
    }

    private void ValidateReferences(string fromMethod)
    {
        Log($"ValidateReferences() from {fromMethod}");

        if (panel == null)
            LogWarning("panel is NULL.");
        else
            Log($"panel = {panel.name}");

        if (noteButtons == null)
            LogWarning("noteButtons is NULL.");
        else
            Log($"noteButtons length = {noteButtons.Length}");

        if (hintText == null)
            LogWarning("hintText is NULL.");
        else
            Log($"hintText = {hintText.name}");

        if (playedText == null)
            LogWarning("playedText is NULL.");
        else
            Log($"playedText = {playedText.name}");

        if (audioSource == null)
            LogWarning("audioSource is NULL.");
        else
            Log($"audioSource = {audioSource.name}");

        if (noteClips == null)
            LogWarning("noteClips is NULL.");
        else
            Log($"noteClips length = {noteClips.Length}");

        if (playerMovementScript == null)
            LogWarning("playerMovementScript is NULL.");
        else
            Log($"playerMovementScript = {playerMovementScript.GetType().Name} on {playerMovementScript.gameObject.name}");

        if (useTargetMelody)
        {
            if (targetMelody == null)
            {
                LogWarning("useTargetMelody is ON, but targetMelody is NULL.");
            }
            else
            {
                Log($"targetMelody length = {targetMelody.Length}");
            }
        }
    }

    private string GetNoteName(int index)
    {
        if (index >= 0 && index < noteNames.Length)
            return noteNames[index];

        return $"Unknown({index})";
    }

    private void Log(string message)
    {
        if (!enableDebugLogs) return;
        Debug.Log($"[PianoMinigameUI] {message}", this);
    }

    private void LogWarning(string message)
    {
        if (!enableDebugLogs) return;
        Debug.LogWarning($"[PianoMinigameUI] {message}", this);
    }

    private void LogError(string message)
    {
        if (!enableDebugLogs) return;
        Debug.LogError($"[PianoMinigameUI] {message}", this);
    }
}