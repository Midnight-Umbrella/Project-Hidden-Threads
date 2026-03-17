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

    [Header("Pickup Popup")]
    [SerializeField] private GameObject cluePopUpPanel;
    [SerializeField] private Image popUpImage;
    [SerializeField] private TMP_Text popUpTitle;
    [SerializeField] private TMP_Text popUpDesc;
    [SerializeField] private KeyCode closeKey = KeyCode.F;
    public bool popUpActive = false;

    [Header("Journal Detail Panel")]
    [SerializeField] private GameObject clueDetailPanel;
    [SerializeField] private Image detailImage;
    [SerializeField] private TMP_Text detailTitle;
    [SerializeField] private TMP_Text detailDesc;
    [SerializeField] private KeyCode detailCloseKey = KeyCode.Escape;
    public bool detailActive = false;

    private void Awake()
    {
        if (cluePopUpPanel != null)
            cluePopUpPanel.SetActive(false);

        if (clueDetailPanel != null)
            clueDetailPanel.SetActive(false);

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (popUpActive && Input.GetKeyDown(closeKey))
        {
            HidePickupPopup();
        }

        if (detailActive && Input.GetKeyDown(detailCloseKey))
        {
            CloseClueDetail();
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

        ShowPickupPopup(clue);
        return true;
    }

    public bool Add(ClueDefinition clue) => AddClue(clue);

    public bool HasClue(string id) => !string.IsNullOrWhiteSpace(id) && _ids.Contains(id);

    public bool Has(string id) => HasClue(id);

    public void OpenClueDetail(ClueDefinition clue)
    {
        if (clue == null)
        {
            Debug.LogWarning("OpenClueDetail called with null clue.");
            return;
        }

        if (clueDetailPanel == null)
        {
            Debug.LogWarning("Clue detail panel is not assigned in inspector.");
            return;
        }

        if (detailTitle != null)
            detailTitle.text = clue.title;

        if (detailDesc != null)
            detailDesc.text = clue.description;

        if (detailImage != null)
        {
            detailImage.sprite = clue.icon;
            detailImage.enabled = clue.icon != null;
        }

        clueDetailPanel.SetActive(true);
        detailActive = true;
    }

    public void CloseClueDetail()
    {
        if (clueDetailPanel != null)
            clueDetailPanel.SetActive(false);

        detailActive = false;
    }

    private void ShowPickupPopup(ClueDefinition clue)
    {
        if (cluePopUpPanel == null)
            return;

        if (popUpTitle != null)
            popUpTitle.text = clue.title;

        if (popUpDesc != null)
            popUpDesc.text = clue.description;

        if (popUpImage != null)
        {
            popUpImage.sprite = clue.icon;
            popUpImage.enabled = clue.icon != null;
        }

        cluePopUpPanel.SetActive(true);
        popUpActive = true;
    }

    private void HidePickupPopup()
    {
        if (cluePopUpPanel != null)
            cluePopUpPanel.SetActive(false);

        popUpActive = false;
    }

    public void ClearAll()
    {
        _ids.Clear();
        _collected.Clear();
        CloseClueDetail();
        HidePickupPopup();
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