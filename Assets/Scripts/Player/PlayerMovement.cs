using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovable
{
    [SerializeField] public float baseMoveSpeed = 5f;
    private Rigidbody2D _rb;
    private Vector2 _movementInput;
    private float _currentSpeed;
    private bool _canMove = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentSpeed = baseMoveSpeed;
    }

    public void Move(Vector2 direction)
    {
        _movementInput = direction;
    }

    public void SetSpeed(float speed)
    {
        _currentSpeed = speed;
    }

    public void EnableMovement(bool enabled)
    {
        _canMove = enabled;
        if (!enabled) _rb.linearVelocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (_canMove)
        {
            _rb.linearVelocity = _movementInput * _currentSpeed;
        }
    }
}
