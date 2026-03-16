using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private int baseEnemiesPerWave = 5;
    [SerializeField] private float enemiesPerWaveMultiplier = 1.3f;
    [SerializeField] private float spawnInterval = 0.5f;

    public int CurrentWave { get; private set; }
    public int EnemiesAlive { get; private set; }

    private EnemySpawner spawner;

    private void Awake()
    {
        spawner = FindObjectOfType<EnemySpawner>();
    }

    public void StartFirstWave()
    {
        CurrentWave = 0;
        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        while (GameManager.Instance != null && !GameManager.Instance.IsGameOver)
        {
            yield return new WaitForSeconds(CurrentWave == 0 ? 2f : timeBetweenWaves);

            CurrentWave++;
            GameManager.Instance.Events?.Fire(new WaveStartedEvent { waveNumber = CurrentWave });

            int count = Mathf.RoundToInt(baseEnemiesPerWave * Mathf.Pow(enemiesPerWaveMultiplier, CurrentWave - 1));
            yield return StartCoroutine(SpawnWave(count));

            while (EnemiesAlive > 0)
                yield return null;

            GameManager.Instance.Events?.Fire(new WaveCompletedEvent { waveNumber = CurrentWave });
        }
    }

    private IEnumerator SpawnWave(int count)
    {
        EnemiesAlive = 0;

        for (int i = 0; i < count; i++)
        {
            EnemyTypeEnum type = (CurrentWave >= 5 && Random.value < 0.3f) ? EnemyTypeEnum.Dasher : EnemyTypeEnum.Grunt;
            Vector2 pos = spawner.GetRandomSpawnPoint();

            GameObject enemyObj = spawner.SpawnEnemy(type, pos);
            if (enemyObj != null)
            {
                EnemiesAlive++;
                var enemy = enemyObj.GetComponent<EnemyBase>();
                if (enemy != null)
                {
                    enemy.OnDeath += () =>
                    {
                        EnemiesAlive--;
                        GameManager.Instance.Events?.Fire(new EnemyKilledEvent { remainingEnemies = EnemiesAlive });
                    };
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
