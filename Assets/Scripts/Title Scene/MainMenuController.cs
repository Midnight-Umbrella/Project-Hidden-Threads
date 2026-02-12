using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string gameSceneName = "GameScene";

    [Header("UI Panels")]
    [SerializeField] private CanvasGroup mainMenuPanel;
    [SerializeField] private CanvasGroup settingsPanel;

    [Header("Transitions")]
    [SerializeField] private float transitionTime = 0.25f;

    [Header("Music")]
    [SerializeField] private AudioClip mainMenuMusic;

    

    void Start()
    {
        ShowMainMenuInstant();

        if (AudioController.Instance != null && mainMenuMusic != null)
        {
            AudioController.Instance.PlayMusic(mainMenuMusic, true);
        }
    }
    // Called by PLAY button
    public void OnPlayPressed()
    {
        SceneManager.LoadScene("OpenCutscene");
    }

    // Called by SETTINGS button
    public void OnSettingsPressed()
    {
        StartCoroutine(Transition(mainMenuPanel, settingsPanel));
    }
    // Called by BACK button (inside settings)
    public void OnBackPressed()
    {
        StartCoroutine(Transition(settingsPanel, mainMenuPanel));
    }

    IEnumerator Transition(CanvasGroup from, CanvasGroup to)
    {
        to.gameObject.SetActive(true);
        to.interactable = false;
        to.blocksRaycasts = false;

        float t = 0f;
        while (t < transitionTime)
        {
            t += Time.deltaTime;
            float a = t / transitionTime;

            from.alpha = 1 - a;
            to.alpha = a;

            yield return null;
        }

        from.alpha = 0;
        from.gameObject.SetActive(false);
        from.interactable = false;
        from.blocksRaycasts = false;

        to.alpha = 1;
        to.interactable = true;
        to.blocksRaycasts = true;
    }

    void ShowMainMenuInstant()
    {
        mainMenuPanel.gameObject.SetActive(true);
        settingsPanel.gameObject.SetActive(false);

        mainMenuPanel.alpha = 1;
        settingsPanel.alpha = 0;

        mainMenuPanel.interactable = true;
        mainMenuPanel.blocksRaycasts = true;
    }
    // Called by EXIT button
    public void OnExitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}


