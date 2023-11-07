using UnityEngine;

public class Followone : MonoBehaviour
{
    public Transform leftWaypoint;
    public Transform rightWaypoint;
    public float moveSpeed = 2.0f;
    private bool isMovingRight = true;
    private Rigidbody2D rb;
    public movements move;
    [SerializeField] private AudioSource Death;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Determine the direction to move
        float horizontalInput = isMovingRight ? 1 : -1;
        Vector2 moveDirection = new Vector2(horizontalInput, 0);

        // Apply movement using the Rigidbody2D
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);

        // Check if the AI should change direction
        if ((isMovingRight && transform.position.x >= rightWaypoint.position.x) ||
            (!isMovingRight && transform.position.x <= leftWaypoint.position.x))
        {
            isMovingRight = !isMovingRight;
            FlipCharacter();
        }
    }

    private void FlipCharacter()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("zone"))
        {
            Death.Play();
            Destroy(gameObject);
          
        }

    }
}

