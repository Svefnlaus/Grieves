using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

        if (other.CompareTag("Boss"))
        {
            StartCoroutine(DamageBoss(other));
        }
    }

    IEnumerator DamageBoss(Collider2D other)
    {
        bold.Play();
        other.TryGetComponent<BossBehavior>(out BossBehavior boss);
        if (boss == null) yield return null;
        boss.TakeDamage(15);
        yield return new WaitForSeconds(0.01f);
        Destroy(gameObject);
        yield return null;
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Destroy the bullet after the specified delay
        Destroy(gameObject);
    }
}


