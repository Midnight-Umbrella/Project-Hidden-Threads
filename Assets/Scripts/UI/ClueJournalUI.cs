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

    private void Awake()
    {
        if (panel != null) panel.SetActive(false);
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
        if (next) Refresh();
    }

    public void Close()
    {
        if (panel != null) panel.SetActive(false);
    }

    private void Refresh()
    {
        if (contentParent == null || clueItemPrefab == null) return;

        // Clear existing items
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        var j = ClueJournal.Instance;
        if (j == null) return;

        if (j.Collected == null || j.Collected.Count == 0)
        {
            // Spawn a single "empty" item
            GameObject empty = Instantiate(clueItemPrefab, contentParent);
            empty.GetComponentInChildren<TMP_Text>().text = "No clues collected yet.";
            return;
        }

        foreach (var clue in j.Collected)
        {
            GameObject item = Instantiate(clueItemPrefab, contentParent);
            TMP_Text[] texts = item.GetComponentsInChildren<TMP_Text>();
            texts[0].text = clue.title;
            if (texts.Length > 1)
                texts[1].text = clue.description;

            Image img = item.transform.Find("ClueImage").GetComponent<Image>();
            if (img != null && clue.icon != null)
                img.sprite = clue.icon;
        }
    }
}