using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movements : MonoBehaviour
{
    public float speed;
    public float jump = 16f;
    public float horizontal;
    private bool isFacingRight = true;
    public Animator animator;

    private bool isdash = true;
    private bool dash;
    private float dashtimer = 0.2f;
    private float dashingcd = 1f;

    private bool canDoubleJump = false; // Added for double jump
    private int remainingJumps = 2;     // Updated for multiple jumps

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundcheck;
    [SerializeField] private LayerMask groundlayer;
    [SerializeField] private TrailRenderer t1;
    [SerializeField] private AudioSource Jump;
    [SerializeField] private AudioSource Hit;
    public float Kbforce;
    public float Kbcounter;
    public float kbtotaltime;
    public bool knockright;

    private bool isFrozen = false; // Added to control player freeze



    private void Update()
    {
        if (dash || isFrozen) // Skip input when dashing or frozen
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("speed", Mathf.Abs(horizontal));

        flip();

        // Check for jump
        if (Input.GetButtonDown("Jump") && !isFrozen)
        {
            animator.SetBool("Jump", true);
            if (IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jump);
                remainingJumps = 1; // Reset jumps when grounded
                canDoubleJump = true;
                Jump.Play();
            }
            else if (remainingJumps > 0) // Perform double jump
            {
                rb.velocity = new Vector2(rb.velocity.x, jump);
                remainingJumps--;
                Jump.Play();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Kbforce = 10;

            // Freeze the player for 1 second
            StartCoroutine(FreezePlayerForDuration(1.0f));
            Hit.Play();
        }
     
        if (collision.gameObject.tag == "Ground")
        {
            animator.SetBool("Jump", false);
        }
    }

    private void FixedUpdate()
    {
        if (dash || isFrozen) // Skip movement when dashing or frozen
        {
            return;
        }

        rb.velocity = new Vector2(speed * horizontal, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundcheck.position, 0.2f, groundlayer);
    }

    private void flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }
    }

    // Coroutine to freeze the player for a specified duration
    private IEnumerator FreezePlayerForDuration(float duration)
    {
        isFrozen = true;
        yield return new WaitForSeconds(duration);
        isFrozen = false;
    }
}
