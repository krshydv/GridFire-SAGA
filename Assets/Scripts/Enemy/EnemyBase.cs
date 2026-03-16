using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBase : MonoBehaviour
{
    [Header("Enemy Config")]
    [SerializeField] protected EnemyStatsSO stats;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    protected Rigidbody2D rb;
    protected Transform target;
    protected float currentHealth;
    protected float attackTimer;
    protected bool isDead;

    public event Action OnDeath;
    public bool IsDead => isDead;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        currentHealth = stats.maxHealth;
    }

    protected virtual void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.Player != null)
            target = GameManager.Instance.Player.transform;
    }

    protected virtual void Update()
    {
        if (isDead || target == null) return;
        attackTimer -= Time.deltaTime;
        HandleAI();
    }

    protected virtual void HandleAI()
    {
        float dist = Vector2.Distance(transform.position, target.position);

        if (dist <= stats.attackRange) Attack();
        else if (dist <= stats.detectionRange) MoveTowardsTarget();
        else rb.linearVelocity = Vector2.zero;
    }

    protected virtual void MoveTowardsTarget()
    {
        Vector2 dir = ((Vector2)target.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * stats.moveSpeed;

        if (spriteRenderer != null)
            spriteRenderer.flipX = dir.x < 0f;
    }

    protected virtual void Attack()
    {
        rb.linearVelocity = Vector2.zero;

        if (attackTimer > 0f) return;
        attackTimer = stats.attackCooldown;

        var playerHP = target.GetComponent<PlayerHealth>();
        playerHP?.TakeDamage((int)stats.damage);
    }

    public virtual void TakeDamage(float damage, float knockback, Vector2 knockbackDir)
    {
        if (isDead) return;

        currentHealth -= damage;
        rb.AddForce(knockbackDir * knockback, ForceMode2D.Impulse);
        StartCoroutine(DamageFlash());

        if (currentHealth <= 0f)
            Die();
    }

    protected virtual void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;

        OnDeath?.Invoke();
        GameManager.Instance?.AddScore(stats.scoreValue);

        AudioManager.Instance?.PlaySFX("enemy_death");
        StartCoroutine(DeathFade());
    }

    private IEnumerator DamageFlash()
    {
        if (spriteRenderer == null) yield break;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        if (!isDead && spriteRenderer != null)
            spriteRenderer.color = Color.white;
    }

    private IEnumerator DeathFade()
    {
        float duration = 0.8f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            if (spriteRenderer != null)
                spriteRenderer.color = new Color(1f, 1f, 1f, 1f - (t / duration));
            yield return null;
        }

        Destroy(gameObject);
    }
}
