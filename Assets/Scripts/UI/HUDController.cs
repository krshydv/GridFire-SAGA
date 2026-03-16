using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image healthFill;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI enemiesText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private PlayerHealth playerHealth;
    private Gun playerGun;
    private bool bound;

    private void Start()
    {
        var events = GameManager.Instance?.Events;
        if (events != null)
        {
            events.Subscribe<WaveStartedEvent>(e => { if (waveText) waveText.text = $"WAVE {e.waveNumber}"; });
            events.Subscribe<EnemyKilledEvent>(e => { if (enemiesText) enemiesText.text = $"Enemies: {e.remainingEnemies}"; });
            events.Subscribe<ScoreChangedEvent>(e => { if (scoreText) scoreText.text = $"Score: {e.score}"; });
        }
    }

    private void Update()
    {
        if (!bound && GameManager.Instance?.Player != null)
        {
            playerHealth = GameManager.Instance.Player.GetComponent<PlayerHealth>();
            playerGun = GameManager.Instance.Player.GetComponentInChildren<Gun>();

            if (playerHealth != null)
            {
                playerHealth.OnHealthChanged += (cur, max) =>
                {
                    if (healthSlider) { healthSlider.maxValue = max; healthSlider.value = cur; }
                    if (healthFill) healthFill.color = Color.Lerp(Color.red, Color.green, (float)cur / max);
                    if (healthText) healthText.text = $"{cur}/{max}";
                };

                if (healthText) healthText.text = $"{playerHealth.CurrentHealth}/{playerHealth.MaxHealth}";
                if (healthSlider) { healthSlider.maxValue = playerHealth.MaxHealth; healthSlider.value = playerHealth.CurrentHealth; }
            }

            if (playerGun != null)
            {
                playerGun.OnClipChanged += (b) => { if (ammoText) ammoText.text = b.ToString(); };
                if (ammoText) ammoText.text = playerGun.BulletsInClip.ToString();
            }

            bound = true;
        }
    }
}
