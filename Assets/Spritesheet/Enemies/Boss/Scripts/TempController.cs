using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempController : MonoBehaviour
{
    [SerializeField] private LayerMask ground;
    private Rigidbody2D rb;
    private Collider2D col;

    private float horizontal
    {
        get
        {
            return Input.GetAxisRaw("Horizontal");
        }
    }

    private bool isGrounded
    {
        get
        {
            float distance = col.bounds.extents.y;
            RaycastHit2D hit = Physics2D.BoxCast(col.bounds.center, col.bounds.size/4, 0f, Vector2.down, distance, ground);

            Color color = hit.collider == null ? Color.red : Color.green;
            Debug.DrawRay(col.bounds.center, Vector2.down * distance, color);
            
            return hit.collider != null;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        if (!isGrounded) return;
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
    }

    private void Move()
    {
        if (!Input.anyKey) return;
        Vector2 motion = rb.velocity;
        motion.x = horizontal * 4;
        rb.velocity = motion;
    }

    private void Jump()
    {
        Vector2 jump = rb.velocity;
        jump.y = 8.15f;
        rb.velocity = jump;
    }
}
