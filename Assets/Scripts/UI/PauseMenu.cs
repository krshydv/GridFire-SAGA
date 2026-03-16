using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private void Start()
    {
        if (pausePanel) pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;

        bool show = GameManager.Instance.IsPaused && !GameManager.Instance.IsGameOver;
        if (pausePanel && pausePanel.activeSelf != show)
            pausePanel.SetActive(show);
    }

    public void OnResumeClicked() => GameManager.Instance?.TogglePause();
    public void OnRestartClicked() => GameManager.Instance?.RestartGame();
    public void OnQuitClicked() => Application.Quit();
}
