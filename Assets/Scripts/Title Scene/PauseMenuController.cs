using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pausePanel;

    [Header("Scene Names")]
    [SerializeField] private string titleSceneName = "TitleScreen";

    private bool _isPaused;

    private void Awake()
    {
        // Ensure starting state
        Time.timeScale = 1f;
        _isPaused = false;
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        if (_isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        _isPaused = true;
        if (pausePanel != null) pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        _isPaused = false;
        if (pausePanel != null) pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Slider：OnValueChanged(float) -> SetVolume
    public void SetVolume(float v)
    {
        v = Mathf.Clamp01(v);
        AudioListener.volume = v;
        PlayerPrefs.SetFloat("master_volume", v);
        PlayerPrefs.Save();
    }

    // Return to title WITHOUT saving this run
    public void ReturnToTitle_NoSave()
    {
        // Always unpause before switching scenes
        Time.timeScale = 1f;
        _isPaused = false;

        // TODO: reset your persistent systems here (inventory/clues/etc) later
        SceneManager.LoadScene(titleSceneName);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        _isPaused = false;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
