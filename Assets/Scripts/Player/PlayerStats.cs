using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "GridFire/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float acceleration = 50f;
    public float deceleration = 40f;

    [Header("Health")]
    public int maxHealth = 100;

    [Header("Invincibility")]
    public float invincibilityDuration = 0.5f;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;
}
