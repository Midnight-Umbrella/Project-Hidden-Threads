using System.Collections;
using UnityEngine;

public class HiddenMelodyTrigger : MonoBehaviour
{
    private bool hasStarted = false;

    public void ShowPopUp()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            gameObject.SetActive(true);
            StartCoroutine(CloseAfterDelay());
        }
    }

    private IEnumerator CloseAfterDelay()
    {
        yield return new WaitForSecondsRealtime(5f);
        gameObject.SetActive(false);
    }
}