using System;
using Interfaces;
using UnityEngine;

public class HoundModel : MonoBehaviour, IMove, IAttack
{
    [SerializeField] private float moveSpeed = 5f;
    public float attackRange;
    
    
    public void ModifySpeed(float speed)
    {
        throw new NotImplementedException();
    }

    public void Move(Vector2 dir)
    {
        transform.Translate(dir.normalized * (moveSpeed * Time.deltaTime));
    }

    public void Dash(float dashForce)
    {
        throw new NotImplementedException();
    }

    public Vector2 Position => transform.position;//Gova
    public void Attack()
    {
        throw new NotImplementedException();
    }

    public Action OnAttack { get; set; }
}
