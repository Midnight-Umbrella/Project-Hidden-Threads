using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ChairToCreditsInteractable : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.F;
    [SerializeField] private GameObject interactPrompt;   // e.g. "Press F to Interact"
    [SerializeField] private string playerTag = "Player";

    [Header("Coming Soon UI")]
    [SerializeField] private GameObject comingSoonPanel;  // panel that says "Feature Coming Soon"
    [SerializeField] private TMP_Text comingSoonText;
    [SerializeField] private string comingSoonMessage = "Feature Coming Soon";
    [SerializeField] private float messageDuration = 1.5f;

    [Header("Scene Transition")]
    [SerializeField] private string creditsSceneName = "Credits Roll Scene";

    private bool playerInRange = false;
    private bool isTransitioning = false;

    private void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        if (comingSoonPanel != null)
            comingSoonPanel.SetActive(false);
    }

    private void Update()
    {
        if (!playerInRange || isTransitioning) return;

        if (Input.GetKeyDown(interactKey))
        {
            StartCoroutine(ShowComingSoonAndLoadCredits());
        }
    }

    private IEnumerator ShowComingSoonAndLoadCredits()
    {
        isTransitioning = true;

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        if (comingSoonPanel != null)
        {
            comingSoonPanel.SetActive(true);

            if (comingSoonText != null)
                comingSoonText.text = comingSoonMessage;
        }

        Debug.Log("Chair interacted. Showing Coming Soon message...");

        yield return new WaitForSeconds(messageDuration);

        Debug.Log("Loading scene: " + creditsSceneName);
        SceneManager.LoadScene(creditsSceneName);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInRange = true;

        if (!isTransitioning && interactPrompt != null)
            interactPrompt.SetActive(true);

        Debug.Log("Player entered chair interaction range.");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInRange = false;

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        Debug.Log("Player exited chair interaction range.");
    }
}