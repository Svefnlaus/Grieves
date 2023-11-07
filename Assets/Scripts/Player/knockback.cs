using UnityEngine;
using System.Collections;

public class knockback : MonoBehaviour
{
    public float knockbackForce = 10.0f; // Adjust the knockback force as needed
    public bool canMove = true; // Control player's ability to move

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!canMove)
        {
            // If the player cannot move, set their velocity to zero.
            rb.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canMove && other.gameObject.CompareTag("Enemy"))
        {
            // Calculate the knockback direction based on the collision
            Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
            // Apply knockback force to the left (change the direction to -1 on the x-axis)
            knockbackDirection.x = -1;
            ApplyKnockback(knockbackDirection);

            // Disable player movement during knockback
            canMove = false;

            // Set a timer to re-enable player movement after a certain time (e.g., 1 second)
            StartCoroutine(EnableMovementAfterDelay(1.0f));
        }
    }

    private void ApplyKnockback(Vector2 direction)
    {
        rb.velocity = Vector2.zero; // Reset the current velocity
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }

    // Coroutine to enable player movement after a delay
    private IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMove = true;
    }
}
