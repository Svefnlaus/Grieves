using UnityEngine;

public class Enemyanim : MonoBehaviour
{
    private Animator animator;
    private Enemyai enemyAI;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyAI = GetComponent<Enemyai>();
    }

    private void Update()
    {
        animator.SetBool("ismoving", true);
    }
}
