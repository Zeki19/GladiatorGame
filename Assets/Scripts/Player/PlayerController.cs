using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    private IMovable _movement;
    private IDashable _dash;
    private IRotatable _rotation;
    private PlayerInputActions _inputActions;
    private Vector2 _moveInput;
    private Vector2 _lookInput;

    private void Awake()
    {
        _movement = GetComponent<IMovable>();
        _dash = GetComponent<IDashable>();
        _rotation = GetComponent<IRotatable>();
        _inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _inputActions.Player.Move.canceled += ctx => _moveInput = Vector2.zero;
        _inputActions.Player.Dash.performed += _ => _dash.Dash();
        _inputActions.Player.Look.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void Update()
    {
        _movement.Move(_moveInput);
        _dash.SetLastDirection(_moveInput);
    }
    private void FixedUpdate()
    {
        _rotation.Rotate(_lookInput);
    }
}
