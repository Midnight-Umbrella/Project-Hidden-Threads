using System.Collections;
using UnityEngine;

public class Stage3PowerManager : MonoBehaviour
{
    public static Stage3PowerManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject darkOverlay;
    [SerializeField] private GameObject miniGamePanel;

    [Header("Optional Lights")]
    [SerializeField] private GameObject[] bulbGlowObjects;

    [Header("State")]
    [SerializeField] private bool powerRestored = false;
    [SerializeField] private float lightTurnOnDelay = 0.15f;

    public bool PowerRestored => powerRestored;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (miniGamePanel != null)
            miniGamePanel.SetActive(false);

        if (powerRestored)
        {
            if (darkOverlay != null)
                darkOverlay.SetActive(false);

            TurnOnAllLightsImmediate();
        }
        else
        {
            if (darkOverlay != null)
                darkOverlay.SetActive(true);

            TurnOffAllLights();
        }
    }

    public void OpenMiniGame()
    {
        if (powerRestored) return;

        if (miniGamePanel != null)
            miniGamePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void CloseMiniGame()
    {
        if (miniGamePanel != null)
            miniGamePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void RestorePower()
    {
        if (powerRestored) return;

        powerRestored = true;
        CloseMiniGame();
        StartCoroutine(RestorePowerRoutine());
    }

    private IEnumerator RestorePowerRoutine()
    {
        if (darkOverlay != null)
            darkOverlay.SetActive(false);

        for (int i = 0; i < bulbGlowObjects.Length; i++)
        {
            if (bulbGlowObjects[i] != null)
                bulbGlowObjects[i].SetActive(true);

            yield return new WaitForSecondsRealtime(lightTurnOnDelay);
        }
    }

    private void TurnOffAllLights()
    {
        for (int i = 0; i < bulbGlowObjects.Length; i++)
        {
            if (bulbGlowObjects[i] != null)
                bulbGlowObjects[i].SetActive(false);
        }
    }

    private void TurnOnAllLightsImmediate()
    {
        for (int i = 0; i < bulbGlowObjects.Length; i++)
        {
            if (bulbGlowObjects[i] != null)
                bulbGlowObjects[i].SetActive(true);
        }
    }
}