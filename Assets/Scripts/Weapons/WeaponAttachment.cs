using UnityEngine;

[CreateAssetMenu(fileName = "NewAttachment", menuName = "GridFire/Weapon Attachment")]
public class WeaponAttachment : ScriptableObject
{
    [Header("Info")]
    public string attachmentName = "New Attachment";
    public string description = "";
    public AttachmentType type;

    [Header("Multipliers (1.0 = no change)")]
    public float fireRateMultiplier = 1f;
    public float damageMultiplier = 1f;
    public float bulletSpeedMultiplier = 1f;

    [Header("Additions")]
    public float spreadAddition = 0f;
    public int additionalBullets = 0;
    public float additionalMultiShotSpread = 0f;

    [Header("Special")]
    public bool enableExplosive = false;
}
