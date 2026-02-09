using UnityEngine;

public class ClueDebugOverlay : MonoBehaviour
{
    public ClueDefinition clueA;
    public ClueDefinition clueB;

    private bool _show = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            _show = !_show;

        var j = ClueJournal.Instance;
        if (j == null) return;

        if (Input.GetKeyDown(KeyCode.Alpha1) && clueA != null)
            j.Add(clueA);

        if (Input.GetKeyDown(KeyCode.Alpha2) && clueB != null)
            j.Add(clueB);
    }

    private void OnGUI()
    {
        if (!_show) return;

        var j = ClueJournal.Instance; 
        if (j == null)
        {
            GUI.Label(new Rect(10, 10, 600, 20), "ClueJournal not initialized (missing ClueJournal in scene).");
            return;
        }

        int y = 10;
        GUI.Label(new Rect(10, y, 800, 20), "F1 toggle | 1 collect clueA | 2 collect clueB");
        y += 24;

        int count = 0;

        foreach (var c in j.All) 
        {
            string title = (c != null && !string.IsNullOrEmpty(c.title)) ? c.title : "(unnamed clue)";
            GUI.Label(new Rect(10, y, 600, 20), $"- {title}");
            y += 20;
            count++;
        }

        if (count == 0)
            GUI.Label(new Rect(10, y, 400, 20), "(no clues collected)");
    }
}
