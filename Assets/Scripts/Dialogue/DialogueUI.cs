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

    [Header("Dialogue Audio")]
    [SerializeField] private AudioClip[] garbleClips; // multiple = variation
    [SerializeField] private AudioSource dialogueAudioSource;
    [SerializeField] private float garbleVolume = 0.5f;
    [SerializeField] private float pitchVariation = 0.2f;
    [SerializeField] private float soundFrequency = 1f; // play every X characters

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

        int charIndex = 0;

        foreach (char c in fullText)
        {
            dialogueText.text += c;
            if (dialogueAudioSource != null && garbleClips.Length > 0 && char.IsWhiteSpace(c) && charIndex % soundFrequency == 0) // Play sound on every 2nd whitespace character for variation
            {
                AudioClip clip = garbleClips[Random.Range(0, garbleClips.Length)];
                dialogueAudioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
                dialogueAudioSource.PlayOneShot(clip, garbleVolume);
                // Reset pitch after playing to avoid affecting future clips
                StartCoroutine(ResetPitchNextFrame());
            }
            charIndex++;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    private IEnumerator ResetPitchNextFrame()
    {
        yield return null;
        dialogueAudioSource.pitch = 1f;
    }
}