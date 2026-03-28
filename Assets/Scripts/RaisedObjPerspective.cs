using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaisedObjPerspective : MonoBehaviour
{
    [SerializeField] GameObject background;
    [SerializeField] GameObject foreground;
    [SerializeField] GameObject chute;
    [SerializeField] GameObject player;
    [SerializeField] float height;
    [SerializeField] float entrance;
    private SpriteRenderer backgroundSR;
    private SpriteRenderer foregroundSR;
    private Collider2D[] catwalkCol;
    private Collider2D[] chuteCol;
    private bool onCatwalk = false;

    void Start()
    {
        backgroundSR = background.GetComponent<SpriteRenderer>();
        foregroundSR = foreground.GetComponent<SpriteRenderer>();

        catwalkCol = background.GetComponents<Collider2D>();
        chuteCol = chute.GetComponents<Collider2D>();

        SetColliderActive(catwalkCol, false);
        SetColliderActive(chuteCol, false);
    }

    private void SetColliderActive(Collider2D[] colArray, bool active)
    {
        foreach(Collider2D col in colArray)
        {
            col.enabled = active;
        }
    }

    void Update()
    {   
        if(!onCatwalk)
        {
            if (player.transform.position.y >= height)
            {
                backgroundSR.sortingOrder = 1;
                foregroundSR.sortingOrder = 1;
            }
            else
            {
                backgroundSR.sortingOrder = -1;
                foregroundSR.sortingOrder = -1;
            }
        }
        else
        {
            backgroundSR.sortingOrder = -1;
            foregroundSR.sortingOrder = 1;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        SetColliderActive(chuteCol, true);
        SetColliderActive(catwalkCol, true);
        onCatwalk = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (player.transform.position.x < entrance)
        {
            SetColliderActive(chuteCol, false);
            SetColliderActive(catwalkCol, false);
            onCatwalk = false;
        }
    }
}
