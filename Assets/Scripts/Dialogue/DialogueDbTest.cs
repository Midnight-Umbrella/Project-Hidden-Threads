using UnityEngine;

public class DialogueDbTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            DialogueManager.Instance?.StartDialogue("INTRO_01");
        }
    }
}