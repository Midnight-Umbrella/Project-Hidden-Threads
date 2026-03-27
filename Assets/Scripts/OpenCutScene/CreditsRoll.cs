using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CreditsRoll : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform creditsText;
    [SerializeField] private TextMeshProUGUI creditsTMP;
    [SerializeField] private RectTransform viewport;

    [Header("Scroll Settings")]
    [SerializeField] private float scrollSpeed = 80f;
    [SerializeField] private float startX = 0f;
    [SerializeField] private float startPadding = 100f;
    [SerializeField] private float endPadding = 100f;

    [Header("Scene Flow")]
    [SerializeField] private string nextSceneName = "TitleScreen";
    [SerializeField] private bool allowSkip = true;

    private float endY;

    private void Start()
    {
        if (creditsText == null)
            creditsText = GetComponent<RectTransform>();

        if (creditsTMP == null)
            creditsTMP = GetComponent<TextMeshProUGUI>();

        if (viewport == null)
            viewport = creditsText.parent as RectTransform;


        creditsTMP.ForceMeshUpdate();

        float preferredHeight = creditsTMP.preferredHeight;
        creditsText.sizeDelta = new Vector2(creditsText.sizeDelta.x, preferredHeight + 50f);


        float autoStartY = -creditsText.sizeDelta.y - startPadding;
        creditsText.anchoredPosition = new Vector2(startX, autoStartY);

        endY = viewport.rect.height + endPadding;
    }

    private void Update()
    {
        creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (allowSkip && (
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetMouseButtonDown(0)))
        {
            LoadNextScene();
        }

        if (creditsText.anchoredPosition.y >= endY)
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}