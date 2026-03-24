using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TransitionSceneController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text loadingText;

    [Header("Timing")]
    [SerializeField] private float minimumLoadingTime = 2f;
    [SerializeField] private float textAnimationSpeed = 0.4f;

    private void Start()
    {
        StartCoroutine(LoadNextSceneRoutine());
        StartCoroutine(AnimateLoadingText());
    }

    private IEnumerator LoadNextSceneRoutine()
    {
        string nextScene = SceneTransitionData.NextSceneName;

        if (string.IsNullOrEmpty(nextScene))
        {
            Debug.LogError("TransitionSceneController: NextSceneName is empty!");
            yield break;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);
        operation.allowSceneActivation = false;

        float timer = 0f;

        while (timer < minimumLoadingTime || operation.progress < 0.9f)
        {
            timer += Time.deltaTime;

            float fakeProgress = Mathf.Clamp01(timer / minimumLoadingTime);
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);

            float displayedProgress = Mathf.Min(fakeProgress, realProgress);

            if (progressBar != null)
                progressBar.value = displayedProgress;

            yield return null;
        }

        if (progressBar != null)
            progressBar.value = 1f;

        yield return new WaitForSeconds(0.2f);

        operation.allowSceneActivation = true;
    }

    private IEnumerator AnimateLoadingText()
    {
        if (loadingText == null) yield break;

        string baseText = "Loading";
        int dots = 0;

        while (true)
        {
            dots = (dots + 1) % 4;
            loadingText.text = baseText + new string('.', dots);
            yield return new WaitForSeconds(textAnimationSpeed);
        }
    }
}