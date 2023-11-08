using System.Collections;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    #region Static Variables

    [Space]

    [SerializeField] private Transform player;
    [Range (0.1f, 50)] [SerializeField] private float speed;
    [Range (0, 1)] [SerializeField] private float trailSize;

    [Space] [Header("Dash Settings")]
    [Range (0.1f, 50)] [SerializeField] private float attackRange;
    [Range (0.1f, 50)] [SerializeField] private float dashSpeed;
    [Range (0.1f, 50)] [SerializeField] private float dashAttackCooldown;
    [Range (0.1f, 50)] [SerializeField] private float dashAttackDuration;

    private Rigidbody2D body;
    private Animator animator;

    #endregion

    #region Dynamic Variables

    private bool isIdle;

    private bool enraged;

    private bool canAttack;
    private bool isAttacking;

    private Vector3 currentVelocity;

    private float scale;
    //private float currentScale;

    private float distance { get { return Vector2.Distance(transform.position, player.position); } }
    private int targetScale { get { return player.position.x < transform.position.x ? -1 : 1; } }

    #endregion

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        enraged = false;
        canAttack = true;
        isAttacking = false;
    }

    private void Update()
    {
        Move();
        Flip();
        Attack();
    }

    private void Flip()
    {
        if (targetScale == scale) return;

        // flip animation
        //scale = Mathf.SmoothDamp(scale, targetScale, ref currentScale, 0.1f * Time.deltaTime, turnSpeed);

        // normal flip
        scale = targetScale;

        transform.localScale = new Vector2(scale, 1);
    }

    private void Attack()
    {
        if (!canAttack) return;
        StartCoroutine(DashAttack());
    }

    private void Move()
    {
        animator.SetBool("IsWalking", body.velocity.magnitude > 0.75f);

        if (distance < attackRange || isAttacking) return;

        Vector3 velocity = body.velocity;
        velocity.x = Mathf.Clamp(player.position.x - transform.position.x, -1, 1);
        body.velocity = velocity * speed * (enraged ? 2 : 1);
    }

    #region Routines

    private IEnumerator BossIdle()
    {
        yield return null;
    }

    private IEnumerator DashAttack()
    {
        canAttack = false;
        isAttacking = true;

        Vector3 targetLocation = player.position;

        TrailRenderer trail = GetComponent<TrailRenderer>();
        trail.time = trailSize;

        float position = Vector3.Distance(transform.position, targetLocation);

        while (position > attackRange)
        {
            position = Vector3.Distance(transform.position, targetLocation);
            transform.position = Vector3.SmoothDamp(transform.position, targetLocation, ref currentVelocity, 0.01f, dashSpeed);
            yield return null;
        }
        animator.SetTrigger("Attack");
        trail.time = 0;

        yield return new WaitForSeconds(dashAttackDuration);
        isAttacking = false;

        yield return new WaitForSeconds(dashAttackCooldown);
        canAttack = true;

        yield return null;
    }

    #endregion
}
