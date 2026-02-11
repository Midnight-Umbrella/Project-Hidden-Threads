using UnityEngine.SceneManagement;

public class TitleController : UnityEngine.MonoBehaviour
{
    public void OnPlayPressed()
    {
        SceneManager.LoadScene("OpenCutscene");
    }
}
