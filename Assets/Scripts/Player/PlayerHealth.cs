using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => stats.maxHealth;
    public bool IsDead => CurrentHealth <= 0;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;
    public event Action OnDamaged;

    private float invincibleTimer;
    private SpriteRenderer[] renderers;

    private void Awake()
    {
        CurrentHealth = stats.maxHealth;
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (invincibleTimer > 0f)
        {
            invincibleTimer -= Time.deltaTime;

            float flash = Mathf.PingPong(Time.time * 15f, 1f) > 0.5f ? 0.3f : 1f;
            SetAlpha(flash);

            if (invincibleTimer <= 0f)
                SetAlpha(1f);
        }
    }

    public void TakeDamage(int damage)
    {
        if (IsDead || invincibleTimer > 0f) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        OnDamaged?.Invoke();

        if (IsDead)
        {
            OnDeath?.Invoke();
            GameManager.Instance?.GameOver();
        }
        else
        {
            SetInvincible(stats.invincibilityDuration);
        }
    }

    public void Heal(int amount)
    {
        if (IsDead) return;
        CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void SetInvincible(float duration)
    {
        invincibleTimer = duration;
    }

    private void SetAlpha(float alpha)
    {
        if (renderers == null) return;

        foreach (var r in renderers)
        {
            var c = r.color;
            c.a = alpha;
            r.color = c;
        }
    }
}
