using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    float speedX, speedY;
    private Rigidbody2D rb;
    public AudioSource playerSound;
    public AudioClip playerFootstep;
    public float footstepInterval = 0.3f;
    private float footstepTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Load the footstep sound
        if (playerSound != null && playerFootstep != null)
        {
            playerSound.clip = playerFootstep; // Only necessary for 'playerSound.Play()'
            // Which we aren't using here
        }
    }

    // Update is called once per frame
    void Update()
    {
        speedX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        speedY = Input.GetAxisRaw("Vertical") * moveSpeed;
        rb.velocity = new Vector2(speedX, speedY);

        HandleFootsteps();
    }

    // Plays footstep sounds with small delay while player is moving
    void HandleFootsteps()
    {
        if (playerSound == null || playerFootstep == null) return;
        bool isMoving = Mathf.Abs(rb.velocity.sqrMagnitude) > 0.01f;

        if (isMoving)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                //PlayOneShot(playerFootstep) is better than Play()
                //In case we want more player noises (e.g. gasp, speak, etc.)
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
