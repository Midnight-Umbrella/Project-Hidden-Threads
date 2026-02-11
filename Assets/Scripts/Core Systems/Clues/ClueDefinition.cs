using UnityEngine;

[CreateAssetMenu(menuName = "HiddenThreads/Clue Definition")]
public class ClueDefinition : ScriptableObject
{
    public string id;          // unique, e.g. clue_note_01
    public string title;       // e.g. "Torn Note"
    [TextArea] public string description;
    public Sprite icon;        // optional
    public string category;    // optional
}
