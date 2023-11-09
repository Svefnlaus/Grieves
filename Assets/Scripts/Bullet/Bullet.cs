using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public AudioSource bold;

    void Start()
    {
        // Start the timer to destroy the bullet after 1 second
        StartCoroutine(DestroyAfterDelay(1));
    }

    void Update()
    {
        // Move the bullet in the forward direction
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet collides with an object tagged as "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Destroy the enemy and the bullet
            bold.Play();
            Destroy(other.gameObject);
            Destroy(gameObject);


        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Destroy the bullet after the specified delay
        Destroy(gameObject);
    }
}


