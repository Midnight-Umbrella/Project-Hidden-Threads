using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [Header("Stage 2 Only")]
    [SerializeField] private GPSTrackerController tc;
    [SerializeField] private ClueDefinition gpsClue;

    void Awake()
    {
        inventory.ResetInventory();
    }

    void Update()
    {
        if (gpsClue != null && inventory.Contains(gpsClue))
        {
            tc.StartTracking();
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        inventory.ResetInventory();
    }
}
