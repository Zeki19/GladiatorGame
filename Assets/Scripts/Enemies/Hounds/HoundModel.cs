using System;
using Interfaces;
using UnityEngine;

public class HoundModel : MonoBehaviour, IMove, IAttack
{
    [Header("Movement Settings")]
    [Tooltip("Speed at which the hound patrols.")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float amountOfWaypoints = 5f;
    
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
        throw new NotImplementedException();
    }

    public void Move(Vector2 dir)
    {
        transform.Translate(dir * (moveSpeed * Time.deltaTime), Space.World);
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
