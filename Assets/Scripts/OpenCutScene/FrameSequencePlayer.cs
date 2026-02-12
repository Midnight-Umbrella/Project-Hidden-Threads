using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FrameSequencePlayer : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Image targetImage;

    [Header("Frames")]
    [SerializeField] private Sprite[] frames;

    [Header("Playback")]
    [SerializeField] private float fps = 8f;             
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private bool loop = false;
    [SerializeField] private bool useUnscaledTime = true;

    [Header("Skip / Finish")]
    [SerializeField] private bool allowSkip = true;
    [SerializeField] private KeyCode skipKey = KeyCode.Space;

    [Tooltip("GameScene")]
    [SerializeField] private string nextSceneName = "";

    private Coroutine _co;

    private void Start()
    {
        if (playOnStart)
            Play();
    }

    private void Update()
    {
        if (!allowSkip) return;

        if (Input.GetKeyDown(skipKey) || Input.GetMouseButtonDown(0))
        {
            Finish();
        }
    }

    public void Play()
    {
        if (_co != null) StopCoroutine(_co);
        _co = StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        if (targetImage == null || frames == null || frames.Length == 0)
            yield break;

        float frameTime = 1f / Mathf.Max(1f, fps);

        do
        {
            for (int i = 0; i < frames.Length; i++)
            {
                targetImage.sprite = frames[i];

                if (useUnscaledTime)
                    yield return new WaitForSecondsRealtime(frameTime);
                else
                    yield return new WaitForSeconds(frameTime);
            }
        } while (loop);

        Finish();
    }

    private void Finish()
    {
        if (_co != null)
        {
            StopCoroutine(_co);
            _co = null;
        }

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
