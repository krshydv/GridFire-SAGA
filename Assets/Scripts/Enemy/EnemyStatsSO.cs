using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "GridFire/Enemy Stats")]
public class EnemyStatsSO : ScriptableObject
{
    [Header("General")]
    public EnemyTypeEnum enemyType;
    public float maxHealth = 30f;
    public float moveSpeed = 3f;
    public float damage = 10f;
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;
    public float detectionRange = 15f;
    public int scoreValue = 100;

    [Header("Dasher Only")]
    public float dashSpeed = 12f;
    public float dashDuration = 0.3f;
    public float dashCooldown = 3f;
}
