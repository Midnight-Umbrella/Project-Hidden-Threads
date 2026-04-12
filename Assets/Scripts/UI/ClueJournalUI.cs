using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClueJournalUI : MonoBehaviour
{
    [Header("UI Refs")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject clueItemPrefab;

    [Header("Optional")]
    [SerializeField] private KeyCode toggleKey = KeyCode.J;

    [Header("Detail Panel")]
    [SerializeField] private GameObject clueDetailPanel;
    [SerializeField] private Image detailImage;
    [SerializeField] private TMP_Text detailTitle;
    [SerializeField] private TMP_Text detailDesc;

    public void OpenDetail(ClueDefinition clue)
    {
        if (clueDetailPanel == null || clue == null) return;
        if (detailTitle != null) detailTitle.text = clue.title;
        if (detailDesc != null) detailDesc.text = clue.description;
        if (detailImage != null) { detailImage.sprite = clue.icon; detailImage.enabled = clue.icon != null; }
        clueDetailPanel.SetActive(true);
    }

    public void CloseDetail()
    {
        if (clueDetailPanel != null)
            clueDetailPanel.SetActive(false);
    }

    private void Awake()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    private void OnEnable()
    {
        if (ClueJournal.Instance != null)
            ClueJournal.Instance.OnChanged += Refresh;
    }

    private void OnDisable()
    {
        if (ClueJournal.Instance != null)
            ClueJournal.Instance.OnChanged -= Refresh;
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            Toggle();
    }

    public void Toggle()
    {
        if (panel == null) return;

        bool next = !panel.activeSelf;
        panel.SetActive(next);

        if (next)
            Refresh();
    }

    public void Close()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    private void Refresh()
    {
        if (contentParent == null || clueItemPrefab == null)
            return;

        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        ClueJournal j = ClueJournal.Instance;
        if (j == null)
            return;

        if (j.Collected == null || j.Collected.Count == 0)
        {
            GameObject empty = Instantiate(clueItemPrefab, contentParent);

            TMP_Text[] emptyTexts = empty.GetComponentsInChildren<TMP_Text>();
            if (emptyTexts.Length > 0)
                emptyTexts[0].text = "No clues collected yet.";
            if (emptyTexts.Length > 1)
                emptyTexts[1].text = "";

            Button emptyButton = empty.GetComponent<Button>();
            if (emptyButton != null)
                emptyButton.interactable = false;

            ClueJournalEntryButton emptyClicker = empty.GetComponent<ClueJournalEntryButton>();
            if (emptyClicker != null)
                emptyClicker.enabled = false;

            Transform emptyImageTf = empty.transform.Find("ClueImage");
            if (emptyImageTf != null)
            {
                Image emptyImg = emptyImageTf.GetComponent<Image>();
                if (emptyImg != null)
                    emptyImg.enabled = false;
            }

            return;
        }

        foreach (ClueDefinition clue in j.Collected)
        {
            GameObject item = Instantiate(clueItemPrefab, contentParent);

            TMP_Text[] texts = item.GetComponentsInChildren<TMP_Text>();
            if (texts.Length > 0)
                texts[0].text = clue.title;
            if (texts.Length > 1)
                texts[1].text = clue.description;

            Transform imageTf = item.transform.Find("ClueImage");
            if (imageTf != null)
            {
                Image img = imageTf.GetComponent<Image>();
                if (img != null)
                {
                    img.sprite = clue.icon;
                    img.enabled = clue.icon != null;
                }
            }

            ClueJournalEntryButton clicker = item.GetComponent<ClueJournalEntryButton>();
            if (clicker == null)
                clicker = item.GetComponentInChildren<ClueJournalEntryButton>();

            if (clicker != null)
                clicker.SetClue(clue);
            else
                Debug.LogWarning("Clue item prefab is missing ClueJournalEntryButton.");
        }
    }
}