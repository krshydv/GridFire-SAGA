using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyPrefabEntry[] enemyPrefabs;
    [SerializeField] private Transform[] spawnPoints;

    private Dictionary<EnemyTypeEnum, GameObject> prefabTable;

    private void Awake()
    {
        prefabTable = new Dictionary<EnemyTypeEnum, GameObject>();
        if (enemyPrefabs == null) return;

        foreach (var entry in enemyPrefabs)
        {
            if (entry.prefab != null)
                prefabTable[entry.type] = entry.prefab;
        }
    }

    public GameObject SpawnEnemy(EnemyTypeEnum type, Vector2 position)
    {
        if (!prefabTable.ContainsKey(type))
        {
            Debug.LogWarning($"[EnemySpawner] No prefab registered for {type}");
            return null;
        }

        return Instantiate(prefabTable[type], position, Quaternion.identity);
    }

    public Vector2 GetRandomSpawnPoint()
    {
        if (spawnPoints != null && spawnPoints.Length > 0)
            return spawnPoints[Random.Range(0, spawnPoints.Length)].position;

        Vector2 playerPos = Vector2.zero;
        if (GameManager.Instance != null && GameManager.Instance.Player != null)
            playerPos = GameManager.Instance.Player.transform.position;

        Vector2 offset = Random.insideUnitCircle.normalized * Random.Range(12f, 18f);
        return playerPos + offset;
    }
}

[System.Serializable]
public class EnemyPrefabEntry
{
    public EnemyTypeEnum type;
    public GameObject prefab;
}
