using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTiles : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            QuitGame();
        }
    }

    void QuitGame()
    {
        Debug.Log("Game Quit Triggered");

        Application.Quit();


        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
