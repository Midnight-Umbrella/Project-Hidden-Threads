using TMPro;
using UnityEngine;

public class CluePromptUI : MonoBehaviour
{
    public static CluePromptUI Instance { get; private set; }

    [SerializeField] private TMP_Text label;

    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(string msg)
    {
        if (label == null) return;
        label.text = msg;
        label.enabled = true;
    }

    public void Hide()
    {
        if (label == null) return;
        label.enabled = false;
    }
}
