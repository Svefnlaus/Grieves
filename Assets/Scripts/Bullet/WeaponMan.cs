using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class WeaponMan : MonoBehaviour
{
    public Rigidbody2D playerRigidbody;
    public Image manaMeter;
    public float manaAmount = 100f;
    public float maxMana = 100f;
    public float manaCost = 20f; // Adjust the mana cost for shooting
    public float regenRate = 5f; // Adjust the mana regen rate per second
    public GameObject projectile; // The projectile prefab to be shot
    public Transform shootPoint; // The point from where the projectile will be shot
    public AudioSource hurting;
    public Animator anim;
    private bool throwfalse = true;

    private float regenTimer = 0f;

    void Update()
    {
        RegenerateMana();

        if (Input.GetKeyDown(KeyCode.K))
        {
            Shoot();

        }
        else
            throwfalse = false;
    }

    void RegenerateMana()
    {
        // Simple mana regeneration
        if (manaAmount < maxMana)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= 1f)
            {
                manaAmount += regenRate;
                manaAmount = Mathf.Clamp(manaAmount, 0, maxMana);
                manaMeter.fillAmount = manaAmount / maxMana;
                regenTimer = 0f;
            }
        }
    }

    void Shoot()
    {
        if (manaAmount >= manaCost)
        {
            // Determine the direction the player is facing
            anim.SetTrigger("throw");
            hurting.Play();
            Vector3 shootDirection = transform.localScale.x > 0 ? Vector3.right : Vector3.left;

            // Instantiate the projectile at the shootPoint position and rotation
            GameObject newBullet = Instantiate(projectile, shootPoint.position, Quaternion.identity);
            Rigidbody2D bulletRB = newBullet.GetComponent<Rigidbody2D>();
            bulletRB.velocity = shootDirection * Mathf.Abs(10); // Set the velocity using the absolute value

            UseMana(manaCost);
            Debug.Log("Shot projectile");

            // Detach the player from the force
            StartCoroutine(DetachPlayerFromForce());
        }
        else
        {
            throwfalse = false;
            Debug.Log("Not enough mana to shoot");
        }
    }

    public void UseMana(float cost)
    {
        manaAmount -= cost;
        manaAmount = Mathf.Clamp(manaAmount, 0, maxMana);
        manaMeter.fillAmount = manaAmount / maxMana;
    }

    public void RestoreMana(float restoreAmount)
    {
        manaAmount += restoreAmount;
        manaAmount = Mathf.Clamp(manaAmount, 0, maxMana);
        manaMeter.fillAmount = manaAmount / maxMana;
    }

      IEnumerator DetachPlayerFromForce()
    {
        playerRigidbody.isKinematic = true;
        yield return new WaitForSeconds(0.1f); // Adjust the time according to your game's needs
        playerRigidbody.isKinematic = false;
    }
}
