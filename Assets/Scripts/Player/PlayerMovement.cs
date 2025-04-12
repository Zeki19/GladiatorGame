using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovable
{
    [SerializeField] public float baseMoveSpeed = 5f;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private float _moveSpeed;
    private float _speedModifier = 1;
    private bool _canMove = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _moveSpeed = baseMoveSpeed;
    }

    public void Move(Vector2 direction)
    {
        _moveInput = direction;
    }

    public void SetSpeed(float speed)
    {
        _moveSpeed = speed;
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
            _rb.linearVelocity = _moveInput * _moveSpeed * _speedModifier;
        }
    }
}
