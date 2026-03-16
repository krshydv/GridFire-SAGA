using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerHealth))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStats stats;
    [SerializeField] private Gun gun;
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private Transform aimPivot;

    private Rigidbody2D rb;
    private PlayerHealth playerHealth;
    private Camera mainCam;

    private Vector2 moveInput;
    private Vector2 currentVelocity;
    private float aimAngle;

    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;
    private Vector2 dashDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
        mainCam = Camera.main;

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        GatherMovementInput();
        HandleAiming();
        HandleShooting();
        HandleDash();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = dashDirection * stats.dashSpeed;
            return;
        }

        ApplyMovement();
    }

    private void GatherMovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(x, y).normalized;
    }

    private void ApplyMovement()
    {
        Vector2 target;
        float rate;

        if (moveInput.sqrMagnitude > 0.01f)
        {
            target = moveInput * stats.moveSpeed;
            rate = stats.acceleration;
        }
        else
        {
            target = Vector2.zero;
            rate = stats.deceleration;
        }

        currentVelocity = Vector2.MoveTowards(currentVelocity, target, rate * Time.fixedDeltaTime);
        rb.linearVelocity = currentVelocity;
    }

    private void HandleAiming()
    {
        if (mainCam == null) return;

        Vector2 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        Vector2 aimDir = (mouseWorld - playerPos).normalized;

        aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

        if (aimPivot != null)
            aimPivot.rotation = Quaternion.Euler(0f, 0f, aimAngle);

        if (bodyRenderer != null)
            bodyRenderer.flipX = Mathf.Abs(aimAngle) > 90f;

        if (gun != null)
            gun.SetAimDirection(aimDir, aimAngle);
    }

    private void HandleShooting()
    {
        if (gun == null) return;

        bool holding = Input.GetButton("Fire1");
        bool clicked = Input.GetButtonDown("Fire1");

        if ((holding && !gun.CurrentStats.isSemiAuto) || clicked)
            gun.TryShoot();

        if (Input.GetKeyDown(KeyCode.R))
            gun.Reload();
    }

    private void HandleDash()
    {
        dashCooldownTimer -= Time.deltaTime;

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
                isDashing = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) &&
            dashCooldownTimer <= 0f &&
            moveInput.sqrMagnitude > 0.01f)
        {
            isDashing = true;
            dashTimer = stats.dashDuration;
            dashCooldownTimer = stats.dashCooldown;
            dashDirection = moveInput;

            playerHealth?.SetInvincible(stats.dashDuration);
        }
    }
}
