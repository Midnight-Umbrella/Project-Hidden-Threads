using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string gameSceneName = "GameScene";

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Transitions")]
    [SerializeField] private float transitionTime = 0.25f;
    [Header("Music")]
    [SerializeField] private AudioClip mainMenuMusic;

    void Start()
    {
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
        if (settingsPanel != null) settingsPanel.SetActive(true);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
    }

    // Called by BACK button (inside settings)
    public void OnBackPressed()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
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
