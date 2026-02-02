using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    public void ReturnToTitle()
    {
        // Use build index to return to main menu
        SceneManager.LoadScene(0);
        
        // if you wanna change name
        // SceneManager.LoadScene("TitleScreen");
    }
}
