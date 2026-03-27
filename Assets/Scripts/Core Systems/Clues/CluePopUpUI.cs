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
    [SerializeField] private float displayDuration = 5f;
    private ClueDefinition waitingClue;

    private Coroutine _hideCoroutine;

    private void Awake()
    {   
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Debug.Log("PopUp exists");
        if (popUpPanel != null)
            popUpPanel.SetActive(false);
    }

    void Update()
    {   
        if (waitingClue != null && !DialogueManager.Instance.IsDialogueActive)
        {
            Show(waitingClue);
            waitingClue = null;
        }
        if (popUpPanel.activeInHierarchy && Input.GetKeyDown(KeyCode.F))
        {
            Hide();
            DialogueManager.Instance.ignoreNextKeyPress = true;
        }
    }

    public void Show(ClueDefinition clue)
    {
        if (clue == null || popUpPanel == null) return;
        if(DialogueManager.Instance.IsDialogueActive)
        {
            waitingClue = clue;
            return;
        }

        if (titleText != null) titleText.text = clue.title;
        if (descriptionText != null) descriptionText.text = clue.description;
        if (clueImage != null)
        {
            clueImage.sprite = clue.icon;
            clueImage.enabled = clue.icon != null;
        }

        DialogueManager.Instance.isDialogueWaiting = true;

        popUpPanel.SetActive(true);

        if (_hideCoroutine != null) StopCoroutine(_hideCoroutine);
        _hideCoroutine = StartCoroutine(HideAfterDelay());
    }
    private void Hide()
    {
        popUpPanel.SetActive(false);
        DialogueManager.Instance.isDialogueWaiting = false;
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        Hide();
    }

}