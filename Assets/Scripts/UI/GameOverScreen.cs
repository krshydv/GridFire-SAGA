using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI finalWaveText;

    private void Start()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);
        GameManager.Instance?.Events?.Subscribe<GameOverEvent>(_ => Show());
    }

    private void Show()
    {
        if (gameOverPanel) gameOverPanel.SetActive(true);

        if (finalScoreText) finalScoreText.text = $"FINAL SCORE: {GameManager.Instance.Score}";
        var wm = FindObjectOfType<WaveManager>();
        if (finalWaveText && wm) finalWaveText.text = $"WAVE: {wm.CurrentWave}";
    }

    public void OnRestartClicked() => GameManager.Instance?.RestartGame();
    public void OnQuitClicked() => Application.Quit();
}
