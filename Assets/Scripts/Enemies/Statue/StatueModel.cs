using Interfaces;
using System;
using UnityEngine;

public class StatueModel : MonoBehaviour, IMove, IAttack
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float attackRange;
    public Vector2 Position => transform.position;
    public float AttackRange => attackRange;
    public void Move(Vector2 dir)
    {
        transform.Translate(dir * (moveSpeed * Time.deltaTime), Space.World);
    }
    public void ModifySpeed(float speed) {    }

    public void Dash(float dashForce) {    }

    public void Attack() {    }

    public Action OnAttack { get; set; }
}
