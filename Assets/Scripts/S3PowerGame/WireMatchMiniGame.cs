using System.Collections;
using TMPro;
using UnityEngine;

public class WireMatchMinigame : MonoBehaviour
{
    public static WireMatchMinigame Instance { get; private set; }

    [Header("References")]
    [SerializeField] private WireNodeButton[] allNodes;
    [SerializeField] private TMP_Text statusText;

    [Header("Settings")]
    [SerializeField] private int totalPairs = 4;
    [SerializeField] private float completeDelay = 0.4f;

    [Header("Audio")]
    [SerializeField] private AudioSource uiAudioSource;

    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip matchClip;
    [SerializeField] private AudioClip wrongClip;

    [SerializeField] private float clickVolume = 1f;
    [SerializeField] private float matchVolume = 1f;
    [SerializeField] private float wrongVolume = 1f;

    private WireNodeButton selectedLeftNode;
    private int matchedPairs = 0;
    private bool completed = false;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        ResetMiniGame();
    }

    public void HandleNodeClicked(WireNodeButton clickedNode)
    {
        if (completed) return;
        if (clickedNode == null) return;
        if (clickedNode.IsMatched) return;

        // 先点左边
        if (clickedNode.Side == WireSide.Left)
        {
            if (selectedLeftNode == clickedNode)
            {
                selectedLeftNode.SetSelected(false);
                selectedLeftNode = null;
                SetStatus("Selection cleared.");
                PlaySFX(wrongClip, wrongVolume);
                return;
            }

            if (selectedLeftNode != null)
                selectedLeftNode.SetSelected(false);

            selectedLeftNode = clickedNode;
            selectedLeftNode.SetSelected(true);
            SetStatus($"Selected {clickedNode.NodeColor}. Now click the matching color on the right.");
            PlaySFX(clickClip, clickVolume);
            return;
        }

        // 点了右边，但还没选左边
        if (selectedLeftNode == null)
        {
            SetStatus("Select a color on the left first.");
            PlaySFX(wrongClip, wrongVolume);
            return;
        }

        // 左右颜色匹配
        if (selectedLeftNode.NodeColor == clickedNode.NodeColor)
        {
            selectedLeftNode.SetSelected(false);
            selectedLeftNode.SetMatched();
            clickedNode.SetMatched();

            selectedLeftNode = null;
            matchedPairs++;

            SetStatus($"Matched {matchedPairs}/{totalPairs}");
            PlaySFX(matchClip, matchVolume);

            if (matchedPairs >= totalPairs)
            {
                completed = true;
                StartCoroutine(FinishRoutine());
            }
        }
        else
        {
            selectedLeftNode.SetSelected(false);
            selectedLeftNode = null;
            SetStatus("Wrong match. Try again.");
            PlaySFX(wrongClip, wrongVolume);
        }
    }

    private IEnumerator FinishRoutine()
    {
        SetStatus("Power restored!");
        yield return new WaitForSecondsRealtime(completeDelay);

        if (Stage3PowerManager.Instance != null)
            Stage3PowerManager.Instance.RestorePower();
    }

    public void ResetMiniGame()
    {
        completed = false;
        matchedPairs = 0;
        selectedLeftNode = null;

        if (allNodes != null)
        {
            for (int i = 0; i < allNodes.Length; i++)
            {
                if (allNodes[i] != null)
                    allNodes[i].ResetNode();
            }
        }

        SetStatus("Match the same colors.");
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }

    private void PlaySFX(AudioClip clip, float volume)
    {
        if (clip == null || AudioController.Instance == null) return;

        if (uiAudioSource != null)
            AudioController.Instance.PlaySFXOnSource(uiAudioSource, clip, volume);
        else
            AudioController.Instance.PlaySFXAtPosition(clip, transform.position, volume);
    }
}