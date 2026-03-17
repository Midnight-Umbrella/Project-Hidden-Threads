using UnityEngine;
using UnityEngine.UI;

public class ClueJournalEntryButton : MonoBehaviour
{
    [SerializeField] private ClueDefinition clue;
    [SerializeField] private Button button;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OpenDetail);
        }
    }

    public void SetClue(ClueDefinition clueData)
    {
        clue = clueData;
    }

    public void OpenDetail()
    {
        if (clue == null)
        {
            Debug.LogWarning($"{name}: no clue assigned.");
            return;
        }

        if (ClueJournal.Instance == null)
        {
            Debug.LogWarning("ClueJournal.Instance is null.");
            return;
        }

        ClueJournal.Instance.OpenClueDetail(clue);
    }
}