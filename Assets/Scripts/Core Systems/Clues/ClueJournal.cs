using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ClueJournal : MonoBehaviour
{
    public static ClueJournal Instance { get; private set; }


    private readonly HashSet<string> _ids = new HashSet<string>();
    private readonly List<ClueDefinition> _collected = new List<ClueDefinition>();

    public event Action OnChanged;

    public IReadOnlyList<ClueDefinition> Collected => _collected;

    public IReadOnlyList<ClueDefinition> All => _collected;

    [SerializeField] private GameObject cluePopUpPanel;
    [SerializeField] private Image popUpImage;
    [SerializeField] private TMP_Text popUpTitle;
    [SerializeField] private TMP_Text popUpDesc;
    [SerializeField] private KeyCode closeKey = KeyCode.F;

    public bool popUpActive = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (cluePopUpPanel != null)
            cluePopUpPanel.SetActive(false);

        // DontDestroyOnLoad(gameObject);
    }

private void Update()
    {
        if (popUpActive && Input.GetKeyDown(closeKey))
        {
            cluePopUpPanel.SetActive(false);
            popUpActive = false;
        }
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


        
        _collected.Add(clue);
        OnChanged?.Invoke();

        if (cluePopUpPanel != null)
        {
            popUpTitle.text = clue.title;
            popUpDesc.text = clue.description;
            
            if (popUpImage != null && clue.icon != null)
                popUpImage.sprite = clue.icon;
            
            cluePopUpPanel.SetActive(true);
            popUpActive = true;
        }

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ClearAll();
    }
}
