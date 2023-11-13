using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossBehavior : MonoBehaviour
{
    #region Static Variables

    [Space] [Header ("Behavior Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject gate;

    [Space] [Header ("Boss Settings")]
    [SerializeField] private BossHealth health;
    [Range (10, 1000)] [SerializeField] private float maxHealth;
    [Space]
    [Range (0.1f, 50)] [SerializeField] private float speed;
    [Range (0.1f, 50)] [SerializeField] private float idleTime;
    [Space]
    [SerializeField] private bool animateFlip;
    [Range(1, 1000)][SerializeField] private int turnSpeed;

    [Space] [Header("Trail Settings")]
    [Range (0.1f, 1)] [SerializeField] private float trailSize;
    [Range (1, 1000)] [SerializeField] private int trailSpeed;

    [Space] [Header ("Attack Settings")]
    [Range (0.1f, 50)] [SerializeField] private float attackRange;
    [Range (0.1f, 50)] [SerializeField] private float attackDamage;
    [SerializeField] private Collider2D attackZone;
    public static float bossNormalAttack;

    [Space] [Header ("Dash Settings")]
    [Range (0.1f, 50)] [SerializeField] private float chargeTime;
    [Range (0.1f, 50)] [SerializeField] private float dashSpeed;
    [Range (0.1f, 50)] [SerializeField] private float dashDamage;
    [Range (0.1f, 50)] [SerializeField] private float dashAttackCooldown;
    [Range (0.1f, 50)] [SerializeField] private float dashAttackDuration;

    private Rigidbody2D body;
    private Animator animator;
    private TrailRenderer trail;
    private ParticleSystem particles;

    #endregion

    #region Dynamic Variables

    private bool isIdle;

    private bool enraged;

    private bool canAttack;
    private bool isAttacking;

    private Vector3 currentVelocity;

    private float currentHealth;

    private float scale;
    private float currentScale;

    private float trailTime;
    private float trailVelocity;
    private bool isFlipping { get { return scale != targetScale; } }
    private bool attacking { get { return animator.GetBool("IsAttacking"); } }
    private float distance
    { 
        get
        {
            if (isIdle) return 0;
            float dis = Vector2.Distance(transform.position, player.position);
            animator.SetBool("IsAttacking", dis <= attackRange);
            attackZone.gameObject.SetActive(attacking);
            return dis;
        }
    }

    private float smoothFlip { get { return Mathf.SmoothDamp(scale, targetScale, ref currentScale, 0.1f * Time.deltaTime, turnSpeed); } }
    private int targetScale { get { return player.position.x < transform.position.x ? -1 : 1; } }

    #endregion

    private bool isDead
    { 
        get
        {
            bool dead = (health.GetComponent<Slider>().value < 0.1f);
            if (dead) gate.SetActive(false);
            if (dead) parent.SetActive(false);
            return dead;
        }
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        trail = GetComponent<TrailRenderer>();

        particles = GetComponentInChildren<ParticleSystem>();

        bossNormalAttack = attackDamage;
    }

    private void Start()
    {
        health.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;

        var duration = particles.main;
        duration.duration = chargeTime;

        isAttacking = false;
        canAttack = true;
        enraged = false;
    }

    private void Update()
    {
        if (isDead || player == null) return;
        Move();
        Flip();
        Attack();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player") || !isAttacking) return;
        other.gameObject.TryGetComponent<Hpmanager>(out Hpmanager player);
        if (player == null) return;
        player.TakeDamage(dashDamage);
        Debug.Log("dash hit");
    }

    // Testing damage
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("zone")) return;
        TakeDamage(5);
    }

    private void Flip()
    {
        if (targetScale == scale || isIdle) return;

        // flip animation
        scale = !animateFlip ? targetScale : smoothFlip;

        transform.localScale = new Vector2(scale, 1);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = (currentHealth <= 0) ? 0 : currentHealth;
        BossHealth.currentHealth = currentHealth;
    }

    private void Trail()
    {
        if (trail.time == trailTime) return;
        trail.time = Mathf.SmoothDamp(trail.time, trailTime, ref trailVelocity, 0.01f * Time.deltaTime, trailSpeed);
    }

    private void Attack()
    {
        if (!canAttack || isIdle || isFlipping) return;
        StartCoroutine(DashAttack());
    }

    private void Move()
    {
        Trail();
        animator.SetBool("IsWalking", body.velocity.magnitude > 0.75f);

        if (distance < attackRange || isAttacking || isIdle || isFlipping) return;

        Vector3 velocity = body.velocity;
        velocity.x = Mathf.Clamp(player.position.x - transform.position.x, -1, 1);
        body.velocity = velocity * speed * (enraged ? 2 : 1);
    }

    #region Routines

    private IEnumerator DashAttack()
    {
        canAttack = false;
        particles.Play();
        isIdle = true;
        yield return new WaitForSeconds(idleTime);
        isIdle = false;

        trailTime = trailSize;

        Vector3 targetLocation = player.position;
        float position = Vector3.Distance(transform.position, targetLocation);

        isAttacking = true;

        while (position >= attackRange)
        {
            position = Vector3.Distance(transform.position, targetLocation);
            transform.position = Vector3.SmoothDamp(transform.position, targetLocation, ref currentVelocity, 0.01f, dashSpeed);
            yield return null;
        }
        trailTime = 0;

        yield return new WaitForSeconds(dashAttackDuration);
        isAttacking = false;

        yield return new WaitForSeconds(dashAttackCooldown);
        canAttack = true;

        yield return null;
    }

    private IEnumerator Overheat()
    {
        yield return null;
    }

    #endregion
}
