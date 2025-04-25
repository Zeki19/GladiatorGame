using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FEESeek<T> : FastEnemyBase<T>
{
    private List<Transform> _waypoints;
    private int _currentWaypointIndex;
    private float _arrivalThreshold = 0.1f;
    private float _moveSpeed;
    private Rigidbody2D _rb;

    public override void Initialize(params object[] args)
    {
        base.Initialize(args);

        _waypoints = args[1] as List<Transform>;
        _moveSpeed = (float)args[5];
        _rb = args[6] as Rigidbody2D;

        _currentWaypointIndex = 0;
    }

    public override void Enter()
    {
        base.Enter();
    }

    //public override void Execute()
    //{
    //    if (_waypoints == null || _waypoints.Count == 0) return;

    //    Transform currentTarget = _waypoints[_currentWaypointIndex];
    //    Vector2 direction = (currentTarget.position - _selfTransform.position).normalized;

    //    _rb.linearVelocity = direction * _moveSpeed;

    //    float distance = Vector2.Distance(_selfTransform.position, currentTarget.position);
    //    if (distance <= _arrivalThreshold)
    //    {
    //        _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Count;
    //    }
    //}

    public override void Exit()
    {
        base.Exit();
        _rb.linearVelocity = Vector2.zero; 
    }
}