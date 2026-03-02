using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    float speedX, speedY;
    private Rigidbody2D rb;

    [Header("Footsteps")]
    public AudioSource playerSound;
    public AudioClip playerFootstep;
    public float footstepInterval = 0.3f;
    private float footstepTimer = 0f;

    [Header("Sprite Animation (No Animator)")]
    public SpriteRenderer sr;          // Drag your SpriteRenderer here
    public Sprite[] walkRight;         // size = 4
    public Sprite[] walkLeft;          // size = 4
    public float animFps = 10f;        // 8~12 feels good for pixel walk
    public float moveEps = 0.01f;      // movement threshold

    private int frameIndex = 0;
    private float frameTimer = 0f;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();

        // Load the footstep sound
        if (playerSound != null && playerFootstep != null)
        {
            playerSound.clip = playerFootstep;
        }

        // Set initial sprite (optional)
        ApplyIdleSprite();
    }

    void Update()
    {
        speedX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        speedY = Input.GetAxisRaw("Vertical") * moveSpeed;
        rb.velocity = new Vector2(speedX, speedY);

        HandleSpriteAnimation();
        HandleFootsteps();
    }

    void HandleSpriteAnimation()
    {
        if (sr == null) return;

        Vector2 v = rb.velocity;
        bool isMoving = v.sqrMagnitude > (moveEps * moveEps);

        // Update facing direction ONLY when there is horizontal movement
        if (Mathf.Abs(v.x) > 0.001f)
            facingRight = v.x > 0f;

        // If not moving: show idle (use first frame of current direction)
        if (!isMoving)
        {
            frameTimer = 0f;
            frameIndex = 0;
            ApplyIdleSprite();
            return;
        }

        // Choose frames by facing
        Sprite[] frames = facingRight ? walkRight : walkLeft;
        if (frames == null || frames.Length == 0) return;

        // Advance frame by time
        float secondsPerFrame = 1f / Mathf.Max(1f, animFps);
        frameTimer += Time.deltaTime;

        while (frameTimer >= secondsPerFrame)
        {
            frameTimer -= secondsPerFrame;
            frameIndex = (frameIndex + 1) % frames.Length;
        }

        // Apply sprite
        sr.sprite = frames[Mathf.Clamp(frameIndex, 0, frames.Length - 1)];
    }

    void ApplyIdleSprite()
    {
        if (sr == null) return;

        // Use the first frame as idle
        Sprite[] frames = facingRight ? walkRight : walkLeft;
        if (frames != null && frames.Length > 0)
            sr.sprite = frames[0];
    }

    // Plays footstep sounds with small delay while player is moving
    void HandleFootsteps()
    {
        if (playerSound == null || playerFootstep == null) return;

        bool isMoving = rb.velocity.sqrMagnitude > 0.01f;

        if (isMoving)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                playerSound.PlayOneShot(playerFootstep);
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }
}