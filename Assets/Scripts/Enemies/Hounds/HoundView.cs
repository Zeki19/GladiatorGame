using System;
using Entities;
using TMPro;
using UnityEngine;

public class HoundView : EntityView, ILook
{
    [SerializeField] private float rotationSpeed;
    
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Run = Animator.StringToHash("Run");
    
    public override void LookDir(Vector2 dir)
    {
        if (dir == Vector2.zero) return;

        float angle = Vector2.SignedAngle(Vector2.up, dir);
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
    
    public override void PlayStateAnimation(StateEnum state)
    {
        switch (state)
        {
            case StateEnum.Idle:
                animator.SetTrigger(Idle);
                break;
            case StateEnum.Attack:
                animator.SetTrigger(Idle);
                break;
            case StateEnum.Chase:
                animator.SetTrigger(Run);
                break;
            case StateEnum.Patrol:
                animator.SetTrigger(Walk);
                break;
            case StateEnum.Runaway:
                animator.SetTrigger(Walk);
                break;
            case StateEnum.Search:
                animator.SetTrigger(Run);
                break;
            case StateEnum.Walk:
            case StateEnum.Default:
            case StateEnum.Dash:
            default:
                Debug.LogWarning("No animation mapped for state: " + state);
                break;
        }
    }
    public override void StopStateAnimation(StateEnum state)
    {
        throw new System.NotImplementedException();
    }
}
