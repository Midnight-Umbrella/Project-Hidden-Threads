using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    private Collider2D cd;
    [SerializeField] Vector3 location;
    [SerializeField] GameObject camera;
    [SerializeField] GameObject fader;
    [SerializeField] float fadeDuration;
    private AudioSource doorAudioSource;
    [Header("Audio")]
    [SerializeField] private AudioClip doorOpenClip;
    [SerializeField] private float sfxVolume = 1f;
    void Start()
    {
        cd = GetComponent<Collider2D>();
        fader.SetActive(false);
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeTo(0f, fadeDuration));
    }

    public void StartFadeIn(GameObject player)
    {
        fader.SetActive(true);
        Renderer renderer = fader.GetComponent<Renderer>();
        Color color = renderer.material.color;
        color.a = 0f;
        renderer.material.color = color;
        StartCoroutine(FadeTo(1f, fadeDuration, player));
    }

    private IEnumerator FadeTo(float targetAlpha, float fadeDuration, GameObject player = null)
    {
        Renderer renderer = fader.GetComponent<Renderer>();

        Color color = renderer.material.color;
        float startAlpha = color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            renderer.material.color = color;
            yield return null;
        }
        
        color.a = targetAlpha;
        renderer.material.color = color;

        if (targetAlpha == 0f)
        {
            fader.SetActive(false);
        }
        else if (targetAlpha == 1f)
        {
            player.transform.position = location;
            camera.transform.position = location;
            StartFadeOut();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !fader.activeInHierarchy)
        {
            StartFadeIn(collision.gameObject);
        }
    }
}
