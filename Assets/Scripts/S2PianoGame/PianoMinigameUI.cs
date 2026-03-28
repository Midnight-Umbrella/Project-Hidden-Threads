using System.Collections;
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

    [Header("Visual Feedback")]
    [SerializeField] private float keyFlashDuration = 0.10f;
    [SerializeField] private Vector3 pressedScale = new Vector3(0.96f, 0.96f, 1f);

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
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    private void Start()
    {
        SetupButtons();
        UpdateHint("Click the keys or press 1-8.");
        UpdatePlayedText();
        Log("Start finished. Buttons bound.");
    }

    private void Update()
    {
        if (!isOpen) return;

        for (int i = 0; i < keyInputs.Length && i < noteClips.Length; i++)
        {
            if (Input.GetKeyDown(keyInputs[i]))
            {
                Log($"Keyboard pressed: {keyInputs[i]} -> {GetNoteName(i)}");
                TriggerNote(i, true);
            }
        }

        if (Input.GetKeyDown(closeKey) || Input.GetKeyDown(altCloseKey))
        {
            Close();
        }
    }

    private void SetupButtons()
    {
        if (noteButtons == null)
        {
            LogWarning("noteButtons is NULL.");
            return;
        }

        for (int i = 0; i < noteButtons.Length; i++)
        {
            int index = i;

            if (noteButtons[i] == null)
            {
                LogWarning($"noteButtons[{i}] is NULL.");
                continue;
            }

            noteButtons[i].onClick.RemoveAllListeners();
            noteButtons[i].onClick.AddListener(() => OnNoteButtonClicked(index));

            Log($"Bound button [{i}] -> {GetNoteName(i)}");
        }
    }

    private void OnNoteButtonClicked(int index)
    {
        Log($"Mouse clicked button index {index} ({GetNoteName(index)})");
        TriggerNote(index, false);
    }

    private void TriggerNote(int index, bool fromKeyboard)
    {
        if (!isOpen)
        {
            LogWarning("TriggerNote ignored because piano is not open.");
            return;
        }

        if (index < 0 || noteClips == null || index >= noteClips.Length)
        {
            LogError($"Invalid note index: {index}");
            return;
        }

        PlayNote(index);

        if (index >= 0 && noteButtons != null && index < noteButtons.Length && noteButtons[index] != null)
        {
            StartCoroutine(FlashButton(noteButtons[index]));
        }

        if (fromKeyboard && noteButtons != null && index < noteButtons.Length && noteButtons[index] != null)
        {

            noteButtons[index].Select();
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
        }

        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        if (useTargetMelody && targetMelody != null && targetMelody.Length > 0)
        {
            UpdateHint("Play the correct melody.");
        }
        else
        {
            UpdateHint("Press 1-8 to play the piano.");
        }

        UpdatePlayedText();
    }

    public void Close()
    {
        Log("Close() called.");

        isOpen = false;

        if (panel != null)
        {
            panel.SetActive(false);
        }

        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }
    }

    public void ClearPlayedNotes()
    {
        playedNotes.Clear();
        UpdateHint("Sequence cleared.");
        UpdatePlayedText();
    }

    private void PlayNote(int index)
    {
        if (!isOpen) return;

        if (audioSource != null && noteClips != null && index >= 0 && index < noteClips.Length && noteClips[index] != null)
        {
            audioSource.PlayOneShot(noteClips[index]);
            Log($"Played sound: {GetNoteName(index)}");
        }
        else
        {
            LogWarning($"Could not play sound for note index {index}. Check audioSource / noteClips.");
        }

        playedNotes.Add(index);
        UpdatePlayedText();

        if (useTargetMelody)
        {
            CheckMelody();
        }
    }

    private void CheckMelody()
    {
        if (isSolved) return;
        if (targetMelody == null || targetMelody.Length == 0) return;

        int currentIndex = playedNotes.Count - 1;

        if (currentIndex >= targetMelody.Length)
        {
            playedNotes.Clear();
            UpdateHint("Wrong melody. Try again.");
            UpdatePlayedText();
            return;
        }

        if (playedNotes[currentIndex] != targetMelody[currentIndex])
        {
            playedNotes.Clear();
            UpdateHint("Wrong melody. Try again.");
            UpdatePlayedText();
            return;
        }

        if (playedNotes.Count == targetMelody.Length)
        {
            isSolved = true;
            UpdateHint("Success!");
            Log("Melody solved.");
            onSolved?.Invoke();
        }
    }

    private IEnumerator FlashButton(Button button)
    {
        if (button == null) yield break;

        RectTransform rect = button.transform as RectTransform;
        if (rect == null) yield break;

        Vector3 originalScale = rect.localScale;
        rect.localScale = pressedScale;

        Image img = button.targetGraphic as Image;
        Color originalColor = Color.white;
        bool hasImage = img != null;

        if (hasImage)
        {
            originalColor = img.color;
            img.color = new Color(originalColor.r * 0.85f, originalColor.g * 0.85f, originalColor.b * 0.85f, originalColor.a);
        }

        yield return new WaitForSeconds(keyFlashDuration);

        rect.localScale = originalScale;

        if (hasImage)
        {
            img.color = originalColor;
        }
    }

    private void UpdateHint(string message)
    {
        if (hintText != null)
        {
            hintText.text = message;
        }
    }

    private void UpdatePlayedText()
    {
        if (playedText == null) return;

        if (playedNotes.Count == 0)
        {
            playedText.text = "Played: (none)";
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
    }

    private string GetNoteName(int index)
    {
        if (index >= 0 && index < noteNames.Length)
            return noteNames[index];

        return $"Unknown({index})";
    }

    private void Log(string message)
    {
        if (enableDebugLogs)
            Debug.Log($"[PianoMinigameUI] {message}", this);
    }

    private void LogWarning(string message)
    {
        if (enableDebugLogs)
            Debug.LogWarning($"[PianoMinigameUI] {message}", this);
    }

    private void LogError(string message)
    {
        if (enableDebugLogs)
            Debug.LogError($"[PianoMinigameUI] {message}", this);
    }
}