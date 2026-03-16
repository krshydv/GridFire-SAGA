using UnityEngine;

[CreateAssetMenu(fileName = "NewGunStats", menuName = "GridFire/Gun Stats")]
public class GunStats : ScriptableObject
{
    [Header("Shooting")]
    public float fireRate = 8f;
    public float bulletSpeed = 20f;
    public float damage = 10f;
    public float spread = 2f;
    public float knockbackAmount = 2f;
    public bool isSemiAuto = false;

    [Header("Clip")]
    public int clipSize = 30;
    public float reloadTime = 1.5f;

    [Header("Multi-shot")]
    public int bulletsPerShot = 1;
    public float multiShotSpread = 10f;

    [Header("Bullet")]
    public float bulletLifetime = 2f;
}
