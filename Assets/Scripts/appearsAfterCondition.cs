using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class appearsAfterCondition : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] ClueDefinition requiredClue;
    private bool isPianoDone;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void pianoDone()
    {
        isPianoDone = true;
    }
    void Update()
    {
        if (inventory.Contains(requiredClue) && isPianoDone)
        {
            gameObject.SetActive(true);
        }
    }
}
