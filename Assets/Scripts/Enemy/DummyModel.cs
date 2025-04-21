using System;
using UnityEngine;

public class DummyModel : MonoBehaviour, IMove, IAttack
{
    public float _speed;
    private Rigidbody _rb;
    
    public float attackRange;
    public LayerMask enemyMask;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Attack()
    {
        var colls = Physics.OverlapSphere(Position, attackRange, enemyMask);
        foreach (var t in colls)
        {
            Destroy(t.gameObject);
        }
    }

    public Action OnAttack { get; set; } = delegate { };


    public void ModifySpeed(float speed)
    {
        _speed = speed;
    }

    public void Move(Vector2 dir, float moveSpeed)
    {
        dir *= _speed;
        dir.y = _rb.linearVelocity.y;
        _rb.linearVelocity = dir;
    }

    public void Dash(float dashForce)
    {
        throw new NotImplementedException();
    }

    public Vector2 Position { get; }
}
