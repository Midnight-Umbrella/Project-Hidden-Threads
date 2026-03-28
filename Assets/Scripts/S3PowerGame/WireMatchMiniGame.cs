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
                return;
            }

            if (selectedLeftNode != null)
                selectedLeftNode.SetSelected(false);

            selectedLeftNode = clickedNode;
            selectedLeftNode.SetSelected(true);
            SetStatus($"Selected {clickedNode.NodeColor}. Now click the matching color on the right.");
            return;
        }

        // 点了右边，但还没选左边
        if (selectedLeftNode == null)
        {
            SetStatus("Select a color on the left first.");
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
}