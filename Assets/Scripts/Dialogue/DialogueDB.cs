using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueLine
{
    public string speaker;
    public string text;
}

public class DialogueDatabase : MonoBehaviour
{
    public static DialogueDatabase Instance { get; private set; }

    private class DialogueEntry
    {
        public int index;
        public DialogueLine line;
    }

    // dialogueId -> ordered lines
    private readonly Dictionary<string, List<DialogueLine>> _db = new();

    [Header("CSV in Resources (no extension)")]
    [SerializeField] private string csvResourcePath = "Dialogue/Dialogue";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadCsv();
    }

    public IReadOnlyList<DialogueLine> GetDialogue(string dialogueId)
    {
        if (_db.TryGetValue(dialogueId, out var lines))
            return lines;

        Debug.LogWarning($"Dialogue id not found: {dialogueId}");
        return Array.Empty<DialogueLine>();
    }

    private void LoadCsv()
    {
        _db.Clear();

        TextAsset csv = Resources.Load<TextAsset>(csvResourcePath);
        if (csv == null)
        {
            Debug.LogError($"Dialogue CSV not found at Resources/{csvResourcePath}.csv");
            return;
        }

        var rows = csv.text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        if (rows.Length <= 1)
        {
            Debug.LogError("Dialogue CSV is empty or missing header.");
            return;
        }

        // Temporary storage with index
        Dictionary<string, List<DialogueEntry>> tempDb = new();

        for (int i = 1; i < rows.Length; i++)
        {
            var cols = SplitCsvRow(rows[i]);
            if (cols.Count < 4) continue;

            string id = cols[0].Trim();
            if (string.IsNullOrEmpty(id)) continue;

            if (!int.TryParse(cols[1].Trim(), out int idx))
                idx = 0;

            string speaker = cols[2].Trim();
            string text = cols[3].Trim();

            if (!tempDb.ContainsKey(id))
                tempDb[id] = new List<DialogueEntry>();

            tempDb[id].Add(new DialogueEntry
            {
                index = idx,
                line = new DialogueLine
                {
                    speaker = speaker,
                    text = text
                }
            });
        }

        // Sort and finalize
        foreach (var pair in tempDb)
        {
            pair.Value.Sort((a, b) => a.index.CompareTo(b.index));

            List<DialogueLine> orderedLines = new();
            foreach (var entry in pair.Value)
            {
                orderedLines.Add(entry.line);
            }

            _db[pair.Key] = orderedLines;
        }

        Debug.Log($"DialogueDatabase loaded {_db.Count} dialogues successfully.");
    }

    // CSV splitter supporting quoted commas
    private static List<string> SplitCsvRow(string row)
    {
        List<string> result = new();
        bool inQuotes = false;
        var current = new System.Text.StringBuilder();

        for (int i = 0; i < row.Length; i++)
        {
            char c = row[i];

            if (c == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }

            if (c == ',' && !inQuotes)
            {
                result.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        result.Add(current.ToString());
        return result;
    }
}