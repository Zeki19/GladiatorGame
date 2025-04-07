using UnityEngine;

public class PlayerDash : MonoBehaviour, IDashable
{
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private PlayerMovement _playerMovement;
    private Rigidbody2D _rb;
    private Vector2 _lastDirection;
    private bool _isDashing;
    private float _dashTimer;
    private float _cooldownTimer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        _cooldownTimer -= Time.deltaTime;

        if (_isDashing)
        {
            _dashTimer -= Time.deltaTime;
            if (_dashTimer <= 0f)
            {
                _isDashing = false;
                _playerMovement.EnableMovement(true);
                _playerMovement.SetSpeed(_playerMovement.baseMoveSpeed);
            }
        }
    }

    public void Dash()
    {
        if (_cooldownTimer > 0 || _isDashing) return;

        _isDashing = true;
        _dashTimer = dashDuration;
        _cooldownTimer = dashCooldown;

        _playerMovement.EnableMovement(false);
        _rb.linearVelocity = _lastDirection.normalized * dashForce;
    }

    public void SetLastDirection(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            _lastDirection = direction;
        }
    }
}
