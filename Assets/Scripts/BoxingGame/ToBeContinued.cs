using System.Collections;
using TMPro;
using UnityEngine;

public class ToBecontinued : MonoBehaviour
{
    [Header("Settings")]
    public TextMeshProUGUI textMesh;
    public string fullText = "To Be Continued...";
    public float typingSpeed = 0.08f;
    public float delayBeforeStart = 1f;

    public void Play()
    {
        StartCoroutine(PlayTypewriterEffect());
    }

    private IEnumerator PlayTypewriterEffect()
    {
        textMesh.text = "";
        yield return new WaitForSeconds(delayBeforeStart);
        foreach (char letter in fullText)
        {
            textMesh.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}