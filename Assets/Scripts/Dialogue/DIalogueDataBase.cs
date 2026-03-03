using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class DialogueDatabase : MonoBehaviour
{
    public static DialogueDatabase Instance { get; private set; }

    [SerializeField] private TextAsset dialogueCsv;

    private readonly Dictionary<string, DialogueRow> _byId = new();
    private readonly Dictionary<string, List<DialogueRow>> _byGroup = new();

    private static readonly Regex TrailingNumber = new(@"_(\d+)$", RegexOptions.Compiled);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Load();
    }

    private void Load()
    {
        _byId.Clear();
        _byGroup.Clear();

        if (dialogueCsv == null)
        {
            Debug.LogError("[DialogueDatabase] dialogueCsv not assigned.");
            return;
        }

        var table = SimpleCsv.Parse(dialogueCsv.text);
        if (table.Count <= 1)
        {
            Debug.LogError("[DialogueDatabase] CSV empty or missing header.");
            return;
        }

        // header indices
        var header = table[0];
        int idxId = IndexOf(header, "ID");
        int idxCharacter = IndexOf(header, "Character");
        int idxType = IndexOf(header, "Type");
        int idxText = IndexOf(header, "Text");
        int idxObject = IndexOf(header, "Object");

        if (idxId < 0 || idxText < 0)
        {
            Debug.LogError("[DialogueDatabase] CSV must include at least ID and Text columns.");
            return;
        }

        for (int i = 1; i < table.Count; i++)
        {
            var r = table[i];
            if (r.Length == 0) continue;

            string id = GetCell(r, idxId).Trim().Trim('\uFEFF');
            if (string.IsNullOrWhiteSpace(id)) continue; // skip blank lines

            var row = new DialogueRow
            {
                id = id,
                character = GetCell(r, idxCharacter).Trim(),
                type = GetCell(r, idxType).Trim(),
                text = GetCell(r, idxText),
                obj = GetCell(r, idxObject).Trim(),
                seq = ParseTrailingNumber(id),
                fileOrder = i
            };

            _byId[id] = row;

            // INTRO_01 -> INTRO, CLUE_KEY_01 -> CLUE_KEY
            string groupKey = RemoveTrailingNumber(id);
            if (!_byGroup.TryGetValue(groupKey, out var list))
            {
                list = new List<DialogueRow>();
                _byGroup[groupKey] = list;
            }
            list.Add(row);
        }

        // sort groups by _01/_02... ; fallback to file order
        foreach (var kv in _byGroup)
        {
            kv.Value.Sort((a, b) =>
            {
                int c = a.seq.CompareTo(b.seq);
                if (c != 0) return c;
                return a.fileOrder.CompareTo(b.fileOrder);
            });
        }

        Debug.Log($"[DialogueDatabase] Loaded: {_byId.Count} rows, {_byGroup.Count} groups.");
    }

    // =========================
    // ✅ 给 DialogueManager 用的
    // =========================
    public IReadOnlyList<DialogueLine> GetDialogue(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) return null;
        key = key.Trim();

        // 1) exact ID -> single line
        if (_byId.TryGetValue(key, out var row))
        {
            return new List<DialogueLine> { ToLine(row) };
        }

        // 2) group key -> sequence
        if (_byGroup.TryGetValue(key, out var list) && list.Count > 0)
        {
            var result = new List<DialogueLine>(list.Count);
            for (int i = 0; i < list.Count; i++) result.Add(ToLine(list[i]));
            return result;
        }

        // 3) allow passing "INTRO_" (rare) -> treat as INTRO
        if (key.EndsWith("_"))
        {
            var trimmed = key.TrimEnd('_');
            if (_byGroup.TryGetValue(trimmed, out var list2) && list2.Count > 0)
            {
                var result2 = new List<DialogueLine>(list2.Count);
                for (int i = 0; i < list2.Count; i++) result2.Add(ToLine(list2[i]));
                return result2;
            }
        }

        return null;
    }

    private static DialogueLine ToLine(DialogueRow r)
    {
        // ⚠️ 这里按你 DialogueLine.cs 里字段名（大写）来赋值
        return new DialogueLine
        {
            Id = r.id,
            Character = r.character,
            Type = r.type,
            Text = r.text,
            ObjectName = r.obj
        };
    }

    // =========================
    // 你原来的 API（保留）
    // =========================
    public bool TryGetLines(string key, out string[] lines)
    {
        lines = null;
        if (string.IsNullOrWhiteSpace(key)) return false;

        key = key.Trim();

        // 1) exact ID -> single line
        if (_byId.TryGetValue(key, out var row))
        {
            lines = new[] { row.text };
            return true;
        }

        // 2) group key -> sequence
        if (_byGroup.TryGetValue(key, out var list) && list.Count > 0)
        {
            lines = new string[list.Count];
            for (int i = 0; i < list.Count; i++) lines[i] = list[i].text;
            return true;
        }

        // 3) allow passing "INTRO_" (rare) -> treat as INTRO
        if (key.EndsWith("_"))
        {
            var trimmed = key.TrimEnd('_');
            if (_byGroup.TryGetValue(trimmed, out var list2) && list2.Count > 0)
            {
                lines = new string[list2.Count];
                for (int i = 0; i < list2.Count; i++) lines[i] = list2[i].text;
                return true;
            }
        }

        return false;
    }

    public bool TryGetRow(string id, out string character, out string type, out string text, out string obj)
    {
        character = type = text = obj = null;
        if (!_byId.TryGetValue(id, out var row)) return false;
        character = row.character;
        type = row.type;
        text = row.text;
        obj = row.obj;
        return true;
    }

    private static int IndexOf(string[] header, string name)
    {
        for (int i = 0; i < header.Length; i++)
            if (string.Equals(header[i].Trim(), name, StringComparison.OrdinalIgnoreCase))
                return i;
        return -1;
    }

    private static string GetCell(string[] row, int idx)
    {
        if (idx < 0) return "";
        if (idx >= row.Length) return "";
        return row[idx] ?? "";
    }

    private static int ParseTrailingNumber(string id)
    {
        var m = TrailingNumber.Match(id);
        if (!m.Success) return 0;
        return int.TryParse(m.Groups[1].Value, out int n) ? n : 0;
    }

    private static string RemoveTrailingNumber(string id)
    {
        return TrailingNumber.Replace(id, "");
    }

    private struct DialogueRow
    {
        public string id;
        public string character;
        public string type;
        public string text;
        public string obj;
        public int seq;
        public int fileOrder;
    }
}

public static class SimpleCsv
{
    public static List<string[]> Parse(string csv)
    {
        var result = new List<string[]>();
        if (string.IsNullOrEmpty(csv)) return result;

        var lines = csv.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
        foreach (var raw in lines)
        {
            if (string.IsNullOrWhiteSpace(raw)) { result.Add(Array.Empty<string>()); continue; }
            result.Add(ParseLine(raw));
        }
        return result;
    }

    private static string[] ParseLine(string line)
    {
        var fields = new List<string>();
        bool inQuotes = false;
        var sb = new StringBuilder();

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    sb.Append('"');
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                fields.Add(sb.ToString());
                sb.Clear();
            }
            else
            {
                sb.Append(c);
            }
        }

        fields.Add(sb.ToString());
        return fields.ToArray();
    }
}