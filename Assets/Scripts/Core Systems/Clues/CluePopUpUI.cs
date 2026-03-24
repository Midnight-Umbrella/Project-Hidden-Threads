using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CluePopUpUI : MonoBehaviour
{
    public static CluePopUpUI Instance { get; private set; }

    [SerializeField] private GameObject popUpPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image clueImage;
    [SerializeField] private float displayDuration = 2.5f;

    private Coroutine _hideCoroutine;

    private void Awake()
    {
        Instance = this;
        if (popUpPanel != null)
            popUpPanel.SetActive(false);
    }

    public void Show(ClueDefinition clue)
    {
        if (clue == null || popUpPanel == null) return;

        if (titleText != null) titleText.text = clue.title;
        if (descriptionText != null) descriptionText.text = clue.description;
        if (clueImage != null)
        {
            clueImage.sprite = clue.icon;
            clueImage.enabled = clue.icon != null;
        }

        popUpPanel.SetActive(true);

        if (_hideCoroutine != null) StopCoroutine(_hideCoroutine);
        _hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        popUpPanel.SetActive(false);
    }
}