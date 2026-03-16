using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [SerializeField] private float followSpeed = 8f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private float maxShakeOffset = 0.3f;

    private Transform target;

    private float shakeDuration;
    private float shakeIntensity;
    private float shakeTimer;

    private void Awake()
    {
        Instance = this;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            var player = GameManager.Instance?.Player;
            if (player != null) target = player.transform;
            else return;
        }

        Vector3 desiredPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);

        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;
            float t = shakeTimer / shakeDuration;
            Vector2 shakeOffset = Random.insideUnitCircle * shakeIntensity * t * maxShakeOffset;
            transform.position += (Vector3)shakeOffset;
        }
    }

    public void Shake(float intensity, float duration)
    {
        shakeIntensity = intensity;
        shakeDuration = duration;
        shakeTimer = duration;
    }
}
