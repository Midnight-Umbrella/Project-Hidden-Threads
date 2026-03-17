using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class appearsAfterCondition : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] List<ClueDefinition> requiredClues = new List<ClueDefinition>();

    void Awake()
    {
        gameObject.SetActive(false);
    }
    void Update()
    {
        bool hasClues = true;
        foreach (ClueDefinition clue in requiredClues)
        {
            if (!inventory.Contains(clue))
            {
                hasClues = false;
                break;
            }
        }

        if (hasClues)
        {
            gameObject.SetActive(true);
        }
    }
}
