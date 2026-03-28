using UnityEngine;
using UnityEngine.SceneManagement;

public class StageExitTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string nextSceneName = "S2GameScene";
    [SerializeField] private string transitionSceneName = "TransitionScene";

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag(playerTag)) return;

        triggered = true;

        SceneTransitionData.NextSceneName = nextSceneName;
        SceneManager.LoadScene(transitionSceneName);
        DialogueManager.Instance.Reset();
    }

    public void manualTransmission()
    {Debug.Log("triggered");
        if (triggered) return;

        triggered = true;

        SceneTransitionData.NextSceneName = nextSceneName;
        SceneManager.LoadScene(transitionSceneName);
        DialogueManager.Instance.Reset();
    }
}