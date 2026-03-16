using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;

    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private WaveManager waveManager;

    public GameObject Player { get; private set; }
    public EventManager Events { get; private set; }

    public int Score { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        Events = GetComponent<EventManager>();
        if (Events == null) Events = gameObject.AddComponent<EventManager>();
    }

    private void Start()
    {
        SpawnPlayer();
        waveManager?.StartFirstWave();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    private void SpawnPlayer()
    {
        if (playerPrefab == null) return;
        Vector3 pos = playerSpawnPoint != null ? playerSpawnPoint.position : Vector3.zero;
        Player = Instantiate(playerPrefab, pos, Quaternion.identity);
    }

    public void AddScore(int points)
    {
        Score += points;
        Events?.Fire(new ScoreChangedEvent { score = Score });
    }

    public void GameOver()
    {
        if (IsGameOver) return;
        IsGameOver = true;
        Time.timeScale = 0f;
        Events?.Fire(new GameOverEvent());
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Instance = null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TogglePause()
    {
        if (IsGameOver) return;
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0f : 1f;
    }
}
