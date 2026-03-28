using UnityEngine;
using TMPro;
using System.Collections;

public class GPSCluePopupUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text codeText;
    [SerializeField] private TMP_Text messageText;

    [Header("Settings")]
    [SerializeField] private float autoHideTime = 4f;

    private Coroutine hideRoutine;

    private void Awake()
    {
        if (root != null)
            root.SetActive(false);
    }

    public void ShowClue(string clueTitle, string melodyCode, string foundMessage)
    {
        if (root != null)
            root.SetActive(true);

        if (titleText != null)
            titleText.text = clueTitle;

        if (codeText != null)
            codeText.text = $"Hidden Numbers: {melodyCode}";

        if (messageText != null)
            messageText.text = foundMessage;

        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        hideRoutine = StartCoroutine(AutoHideRoutine());
    }

    public void Hide()
    {
        if (root != null)
            root.SetActive(false);
    }

    private IEnumerator AutoHideRoutine()
    {
        yield return new WaitForSeconds(autoHideTime);
        Hide();
    }
}