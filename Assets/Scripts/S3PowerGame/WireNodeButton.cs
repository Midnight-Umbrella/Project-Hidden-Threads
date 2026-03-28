using UnityEngine;
using UnityEngine.UI;

public enum WireColor
{
    Orange,
    Green,
    Red,
    Blue
}

public enum WireSide
{
    Left,
    Right
}

public class WireNodeButton : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private WireColor wireColor;
    [SerializeField] private WireSide wireSide;
    [SerializeField] private Button button;
    [SerializeField] private Image hotspotImage;

    [Header("Visual")]
    [SerializeField] private Color normalColor = new Color(1f, 1f, 1f, 0.08f);
    [SerializeField] private Color selectedColor = new Color(1f, 1f, 1f, 0.35f);
    [SerializeField] private Color matchedColor = new Color(1f, 1f, 0.4f, 0.6f);

    private bool isMatched = false;

    public WireColor NodeColor => wireColor;
    public WireSide Side => wireSide;
    public bool IsMatched => isMatched;

    private void Reset()
    {
        button = GetComponent<Button>();
        hotspotImage = GetComponent<Image>();
    }

    private void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        if (hotspotImage == null) hotspotImage = GetComponent<Image>();

        ApplyColor(normalColor);
    }

    public void OnClickNode()
    {
        if (isMatched) return;

        WireMatchMinigame.Instance?.HandleNodeClicked(this);
    }

    public void ResetNode()
    {
        isMatched = false;

        if (button != null)
            button.interactable = true;

        ApplyColor(normalColor);
    }

    public void SetSelected(bool selected)
    {
        if (isMatched) return;

        ApplyColor(selected ? selectedColor : normalColor);
    }

    public void SetMatched()
    {
        isMatched = true;

        if (button != null)
            button.interactable = false;

        ApplyColor(matchedColor);
    }

    private void ApplyColor(Color color)
    {
        if (hotspotImage != null)
            hotspotImage.color = color;
    }
}