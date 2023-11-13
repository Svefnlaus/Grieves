using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hpmanager : MonoBehaviour
{
    public Image hpmeter;
    public float hpamount = 100f;
    public float knockbackForce = 5f; // Adjust the knockback force as needed
    public AudioSource heal;
    


    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(10);

            Debug.Log("Damage taken");
        }

        if (collision.gameObject.CompareTag("Firstaid"))
        {
            if (hpamount < 100)
            {
                heal.Play();
                Heal(20); // Adjust the healing amount as needed
                Destroy(collision.gameObject); // Destroy the first aid kit
                Debug.Log("Healed");
            }
            else
            {
              
                Debug.Log("HP is already full");
            }


        }
    }

    public void TakeDamage(float damageAmount)
    {
        hpamount -= damageAmount;
        hpamount = Mathf.Clamp(hpamount, 0, 100);
        hpmeter.fillAmount = hpamount / 100f;

        if (hpamount <= 0)
        {
            Die(); // Call a function to handle player death
          
            Destroy(gameObject);
        }
    }

    void Heal(float healAmount)
    {
        hpamount += healAmount;
        hpamount = Mathf.Clamp(hpamount, 0, 100);
        hpmeter.fillAmount = hpamount / 100f;
    }

    void Die()
    {
        // Handle player death here, such as showing a game over screen or restarting the level.
        Debug.Log("Player has died");
        // You can add more code here to handle the death scenario.
    }
}
