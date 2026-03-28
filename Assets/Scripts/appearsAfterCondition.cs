using UnityEngine;

public class appearsAfterCondition : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private ClueDefinition requiredClue;

    [Header("Hide / Show")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D interactionCollider;

    private bool isPianoDone;
    private bool hasAppeared;

    private void Awake()
    {
        SetVisible(false);
    }

    public void pianoDone()
    {
        Debug.Log("Louise pianoDone called");
        isPianoDone = true;
        TryAppear();
    }

    private void Update()
    {
        if (!hasAppeared)
        {
            TryAppear();
        }
    }

    private void TryAppear()
    {
        if (!isPianoDone) return;

        if (requiredClue == null)
        {
            SetVisible(true);
            Debug.Log("Louise appeared: piano only.");
            return;
        }

        if (inventory != null && inventory.Contains(requiredClue))
        {
            SetVisible(true);
            Debug.Log("Louise appeared: piano + required clue.");
        }
    }

    private void SetVisible(bool visible)
    {
        hasAppeared = visible;

        if (spriteRenderer != null)
            spriteRenderer.enabled = visible;

        if (interactionCollider != null)
            interactionCollider.enabled = visible;
    }
}