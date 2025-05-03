using Entities;
using UnityEngine;

public class Boss1Model : EntityModel
{
    [Header("Boss Movement")]
    [SerializeField] private float baseSpeed = 2f;

    private float _currentSpeed;

    private void Awake()
    {
        _currentSpeed = baseSpeed;
    }

    public override void ModifySpeed(float speed)
    {
        _currentSpeed = speed;
    }

    public override void Move(Vector2 dir)
    {
        manager.Rb.linearVelocity = dir.normalized * _currentSpeed;
    }

    public override void Dash(float dashForce)
    {
        Vector2 dashDir = manager.Rb.linearVelocity.normalized;
        manager.Rb.AddForce(dashDir * dashForce, ForceMode2D.Impulse);
    }

    public void Stop()
    {
        manager.Rb.linearVelocity = Vector2.zero;
    }
}
