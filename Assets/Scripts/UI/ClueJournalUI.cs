using System.Text;
using TMPro;                 // 如果你用的是 TextMeshPro
using UnityEngine;

public class ClueJournalUI : MonoBehaviour
{
    [Header("UI Refs")]
    [SerializeField] private GameObject panel;   
    [SerializeField] private TMP_Text listText;  

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
        var j = ClueJournal.Instance;
        if (j == null || listText == null) return;

        if (j.Collected == null || j.Collected.Count == 0)
        {
            listText.text = "(no clues collected)";
            return;
        }

        var sb = new StringBuilder();
        for (int i = 0; i < j.Collected.Count; i++)
        {
            var c = j.Collected[i];
            sb.AppendLine($"{i + 1}. {c.title}");

            if (!string.IsNullOrWhiteSpace(c.description))
                sb.AppendLine($"   {c.description}");

            sb.AppendLine();
        }

        listText.text = sb.ToString();
    }
}
