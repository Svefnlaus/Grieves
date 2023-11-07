using UnityEngine;

public class Enemyai : MonoBehaviour
{
    public Transform target; // The player's transform, which the AI will follow
    public float moveSpeed = 3f; // The speed at which the AI moves
    public float chaseDistance = 5f; // The maximum distance at which the AI will chase the player
    public int damageAmount = 10; // The amount of damage the enemy deals to the player
    public float flipCooldown = 1f; // Time in seconds before the enemy can flip again
    public float continuousDamageRate = 5f; // Damage per second when in contact with the player

    private Rigidbody2D rb;
    private bool isFacingRight = true; // Flag to track the enemy's facing direction
    private float flipTimer = 0f; // Timer to control flipping
    private bool isContinuousDamageActive = false; // Flag to track continuous damage
    public GameObject leech;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        leech.SetActive(false);
    }

    private void Update()
    {
        if (target != null)
        {
            // Calculate the direction from AI to the player
            Vector2 moveDirection = (target.position - transform.position).normalized;

            // Calculate the distance between the AI and the player
            float distanceToPlayer = Vector2.Distance(transform.position, target.position);

            // Flip the enemy based on the player's position
            if (moveDirection.x > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (moveDirection.x < 0 && isFacingRight)
            {
                Flip();
            }

            if (distanceToPlayer <= chaseDistance)
            {
                // Move the AI towards the player
                rb.velocity = moveDirection * moveSpeed;

                // Enable continuous damage if in contact with the player
                if (isContinuousDamageActive)
                {
                    leech.SetActive(true);
                    ApplyContinuousDamage();
                }
            }
            else
            {
                rb.velocity = Vector2.zero; // Stop moving if the player is too far away
            }
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop moving if the target is null
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           
            Hpmanager playerHealth = collision.GetComponent<Hpmanager>();
            if (playerHealth != null)
            {
           
                playerHealth.TakeDamage(damageAmount);

          
                isContinuousDamageActive = true;
            }
        }
        if (collision.gameObject.CompareTag("zone"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Disable continuous damage when not in contact with the player
            leech.SetActive(false);
            isContinuousDamageActive = false;
        }
    }

    private void ApplyContinuousDamage()
    {
        // Apply continuous damage to the player
        Hpmanager playerHealth = target.GetComponent<Hpmanager>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount * Time.deltaTime * continuousDamageRate);
        }
    }

    private void Flip()
    {
        // Flip the enemy's sprite and update the facing direction
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
