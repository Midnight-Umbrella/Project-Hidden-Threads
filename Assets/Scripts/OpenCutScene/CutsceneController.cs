using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "GameScene";
    [SerializeField] private KeyCode skipKey = KeyCode.Space;

    [Header("Assign this if the VideoPlayer is on another object")]
    [SerializeField] private VideoPlayer vp;

    private bool _loading;

    private void Awake()
    {
        if (vp == null) vp = GetComponent<VideoPlayer>();
        if (vp == null) vp = FindObjectOfType<VideoPlayer>(); 

        if (vp == null)
        {
            Debug.LogError("CutsceneController: No VideoPlayer found in scene.");
            return;
        }


        vp.isLooping = false;

        vp.loopPointReached += _ => LoadNext();

        Debug.Log("CutsceneController ready. VideoPlayer found on: " + vp.gameObject.name);
    }

    private void Update()
    {
        if (!_loading && Input.GetKeyDown(skipKey))
        {
            Debug.Log("Skip pressed.");
            LoadNext();
        }

        if (!_loading && vp != null && vp.isPrepared && !vp.isPlaying && vp.time > 0.1f)
        {
            Debug.Log("Video stopped; loading next scene.");
            LoadNext();
        }
    }

    private void LoadNext()
    {
        if (_loading) return;
        _loading = true;

        Debug.Log("Loading scene: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }
}
