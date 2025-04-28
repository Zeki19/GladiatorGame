using System;
using Interfaces;
using UnityEngine;

public class HoundModel : MonoBehaviour, IMove, IAttack
{
    [Header("Movement Settings")]
    [Tooltip("Speed at which the hound patrols.")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float amountOfWaypoints = 5f;
    private float _speedMult = 1f;
    
    
    
    [Header("Attack Settings")]
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;

    #region Public calls to private variables
    public Vector2 Position => transform.position;
    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;
    public float AmountOfWaypoints => amountOfWaypoints;

    #endregion

    public void ModifySpeed(float speed)
    {
        _speedMult = speed;
    }

    public void Move(Vector2 dir)
    {
        if (_speedMult <= 0) _speedMult = 1;
        //transform.Translate(dir * (moveSpeed * _speedMult * Time.deltaTime), Space.World);
        _rb.linearVelocity =  dir * (moveSpeed * _speedMult);
    }

    public void Dash(float dashForce)
    {
        throw new NotImplementedException();
    }
    
    public void Attack()
    {
        throw new NotImplementedException();
    }

    public Action OnAttack { get; set; }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,attackRange);
    }
}
