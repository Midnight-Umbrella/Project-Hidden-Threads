using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HiddenThreads/Inventory")]
public class Inventory : ScriptableObject
{
    public List<ClueDefinition> clues = new List<ClueDefinition>();
    public void AddClue(ClueDefinition clue)
    {
        clues.Add(clue);
    }

    public bool Contains(ClueDefinition clue)
    {
        if (clues.Contains(clue))
        {
            return true;
        }
        return false;
    }

    public void ResetInventory()
    {
        clues.Clear();
    }
}
