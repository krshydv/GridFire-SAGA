using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunStats baseStats;
    [SerializeField] private Transform firePoint;
    [SerializeField] private BulletPool bulletPool;
    [SerializeField] private SpriteRenderer gunRenderer;
    [SerializeField] private GameObject muzzleFlashPrefab;

    private float timeSinceLastShot;
    private int bulletsInClip;
    private bool isReloading;
    private float reloadTimer;

    private float aimAngle;

    private float modFireRate;
    private float modDamage;
    private float modSpread;
    private float modBulletSpeed;
    private int modBulletsPerShot;
    private float modMultiShotSpread;
    private bool modExplosive;

    public GunStats CurrentStats => baseStats;
    public int BulletsInClip => bulletsInClip;
    public bool IsReloading => isReloading;
    public float ReloadProgress => isReloading ? (reloadTimer / baseStats.reloadTime) : 0f;

    public event Action<int> OnClipChanged;
    public event Action OnReloadStart;
    public event Action OnReloadEnd;

    private void Awake()
    {
        ResetModifiers();
        bulletsInClip = baseStats.clipSize;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer >= baseStats.reloadTime)
                FinishReload();
        }

        UpdateGunVisuals();
    }

    public void SetAimDirection(Vector2 dir, float angle)
    {
        aimAngle = angle;
    }

    public void TryShoot()
    {
        if (isReloading) return;

        if (bulletsInClip <= 0)
        {
            Reload();
            return;
        }

        if (timeSinceLastShot < 1f / modFireRate) return;

        timeSinceLastShot = 0f;

        FireBullets();
        bulletsInClip--;
        OnClipChanged?.Invoke(bulletsInClip);

        CameraController.Instance?.Shake(0.05f, 0.1f);

        if (muzzleFlashPrefab != null && firePoint != null)
        {
            var flash = Instantiate(muzzleFlashPrefab, firePoint.position, Quaternion.Euler(0f, 0f, aimAngle));
            Destroy(flash, 0.1f);
        }

        AudioManager.Instance?.PlaySFX("shoot");

        if (bulletsInClip <= 0)
            Reload();
    }

    private void FireBullets()
    {
        int count = modBulletsPerShot;

        if (count <= 1)
        {
            FireOneBullet(aimAngle);
        }
        else
        {
            float total = modMultiShotSpread;
            float start = aimAngle - total * 0.5f;
            float step = total / (count - 1);

            for (int i = 0; i < count; i++)
                FireOneBullet(start + step * i);
        }
    }

    private void FireOneBullet(float baseAngle)
    {
        if (bulletPool == null)
        {
            Debug.LogError("[Gun] BulletPool is not assigned!");
            return;
        }

        float offset = UnityEngine.Random.Range(-modSpread, modSpread);
        float angle = baseAngle + offset;
        float rad = angle * Mathf.Deg2Rad;

        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        Vector2 pos = firePoint != null ? (Vector2)firePoint.position : (Vector2)transform.position;

        Bullet bullet = bulletPool.GetBullet();
        bullet.Initialize(pos, dir, modBulletSpeed, modDamage, baseStats.knockbackAmount, baseStats.bulletLifetime, modExplosive);
    }

    public void Reload()
    {
        if (isReloading) return;
        if (bulletsInClip >= baseStats.clipSize) return;

        isReloading = true;
        reloadTimer = 0f;
        OnReloadStart?.Invoke();
        AudioManager.Instance?.PlaySFX("reload");
    }

    private void FinishReload()
    {
        isReloading = false;
        bulletsInClip = baseStats.clipSize;
        OnClipChanged?.Invoke(bulletsInClip);
        OnReloadEnd?.Invoke();
    }

    private void UpdateGunVisuals()
    {
        if (gunRenderer == null) return;
        bool flipped = Mathf.Abs(aimAngle) > 90f;
        gunRenderer.flipY = flipped;
    }

    public void ResetModifiers()
    {
        modFireRate = baseStats.fireRate;
        modDamage = baseStats.damage;
        modSpread = baseStats.spread;
        modBulletSpeed = baseStats.bulletSpeed;
        modBulletsPerShot = baseStats.bulletsPerShot;
        modMultiShotSpread = baseStats.multiShotSpread;
        modExplosive = false;
    }

    public void ApplyAttachment(WeaponAttachment attachment)
    {
        if (attachment == null) return;

        modFireRate *= attachment.fireRateMultiplier;
        modDamage *= attachment.damageMultiplier;
        modBulletSpeed *= attachment.bulletSpeedMultiplier;
        modSpread += attachment.spreadAddition;
        modBulletsPerShot += attachment.additionalBullets;
        modMultiShotSpread += attachment.additionalMultiShotSpread;
        if (attachment.enableExplosive) modExplosive = true;
    }
}
