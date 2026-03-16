using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject explosionPrefab;

    private Rigidbody2D rb;
    private float damage;
    private float knockback;
    private float lifetime;
    private float timer;
    private bool isExplosive;
    private bool isActive;
    private BulletPool pool;

    public void SetPool(BulletPool bulletPool) => pool = bulletPool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        var col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;
    }

    public void Initialize(Vector2 position, Vector2 direction, float speed,
        float dmg, float kb, float life, bool explosive)
    {
        transform.position = position;
        damage = dmg;
        knockback = kb;
        lifetime = life;
        isExplosive = explosive;
        timer = 0f;
        isActive = true;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        rb.linearVelocity = direction.normalized * speed;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!isActive) return;

        timer += Time.deltaTime;
        if (timer >= lifetime)
            ReturnToPool();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;

        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<EnemyBase>();
            enemy?.TakeDamage(damage, knockback, rb.linearVelocity.normalized);

            AudioManager.Instance?.PlaySFX("enemy_hit");

            if (isExplosive) Explode();
            else SpawnHitEffect();

            ReturnToPool();
        }
        else if (other.CompareTag("Wall"))
        {
            if (isExplosive) Explode();
            else SpawnHitEffect();
            ReturnToPool();
        }
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            var fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 0.5f);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.5f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                var enemy = hit.GetComponent<EnemyBase>();
                if (enemy != null)
                {
                    Vector2 dir = ((Vector2)hit.transform.position - (Vector2)transform.position).normalized;
                    enemy.TakeDamage(damage * 0.5f, knockback * 2f, dir);
                }
            }
        }

        CameraController.Instance?.Shake(0.15f, 0.2f);
        AudioManager.Instance?.PlaySFX("explosion");
    }

    private void SpawnHitEffect()
    {
        if (hitEffectPrefab != null)
        {
            var fx = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 0.3f);
        }
    }

    private void ReturnToPool()
    {
        isActive = false;
        rb.linearVelocity = Vector2.zero;
        gameObject.SetActive(false);
        pool?.Return(this);
    }
}
