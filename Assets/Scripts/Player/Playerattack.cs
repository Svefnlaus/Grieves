using System.Collections;
using UnityEngine;

public class Playerattack : MonoBehaviour
{
    public GameObject attackArea;
    private bool attacking = false;
    public Animator anim;
    private float timeToAttack = 0.25f;
    private float timer = 0f;
    [SerializeField] private AudioSource Attack;
    [SerializeField] private AudioSource death;
    // Start is called before the first frame update
    void Start()
    {
        // Disable the attack area initially
        attackArea.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && !attacking)
        {
            StartCoroutine(PerformAttack());
       
            
        }
        
    }

    IEnumerator PerformAttack()
    {
        attacking = true;
        Attack.Play();

        anim.SetTrigger("attack");

        // Wait for the specified delay before spawning the attack area
        yield return new WaitForSeconds(timeToAttack);

        // Enable the attack area
        attackArea.SetActive(true);

        // Wait for a short duration to match the animation (adjust as needed)
        yield return new WaitForSeconds(0.1f);

        // Disable the attack area
        attackArea.SetActive(false);

        attacking = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            death.Play();
        }
    }
}
