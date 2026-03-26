using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClueJournal : MonoBehaviour
{
    public static ClueJournal Instance { get; private set; }

    private readonly HashSet<string> _ids = new HashSet<string>();
    private readonly List<ClueDefinition> _collected = new List<ClueDefinition>();

    public event Action OnChanged;

    public IReadOnlyList<ClueDefinition> Collected => _collected;
    public IReadOnlyList<ClueDefinition> All => _collected;

    public bool popUpActive = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public bool AddClue(ClueDefinition clue)
    {
        if (clue == null) return false;

        if (string.IsNullOrWhiteSpace(clue.id))
        {
            Debug.LogWarning("ClueDefinition.id is empty.");
            return false;
        }

        if (!_ids.Add(clue.id)) return false;
        Debug.Log("Added to journal");
        CluePromptUI.Instance?.Hide();
        CluePopUpUI.Instance?.Show(clue);

        _collected.Add(clue);
        OnChanged?.Invoke();
        return true;
    }

    public bool Add(ClueDefinition clue) => AddClue(clue);
    public bool HasClue(string id) => !string.IsNullOrWhiteSpace(id) && _ids.Contains(id);
    public bool Has(string id) => HasClue(id);

    public void ClearAll()
    {
        _ids.Clear();
        _collected.Clear();
        OnChanged?.Invoke();
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => ClearAll();
}