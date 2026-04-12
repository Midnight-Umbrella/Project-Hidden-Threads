using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothDelay = .15f;
    private Vector3 velocity = Vector3.zero;
    public bool focusing;  //摄像机是否聚焦在玩家身上的标志位

    [SerializeField] private Transform player;

    private void Awake()
    {
        focusing = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (focusing)
        {
            Vector3 playerPos = player.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref velocity, smoothDelay);
        }
    }
}
