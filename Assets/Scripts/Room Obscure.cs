using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObscure : MonoBehaviour
{
    public float fadeDuration;

    private SpriteRenderer sr;
    private Collider2D cd;
    private bool fading = false;



    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D player)
    {
        if (!fading && player.CompareTag("Player"))
        {
            StartCoroutine(FadeOut());
        }
    }


    System.Collections.IEnumerator FadeOut()
    {
        fading = true;

        float startAlpha = sr.color.a;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, t / fadeDuration);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            yield return null;
        }
        
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);

        gameObject.SetActive(false);
    }
}
