using System;
using Unity.Mathematics;
using UnityEngine;

public class GaiusView : EnemyView, ILook
{
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Stun = Animator.StringToHash("Stun");
    private static readonly int Swing = Animator.StringToHash("Swing");
    private static readonly int Lunge = Animator.StringToHash("Lunge");
    private static readonly int Move = Animator.StringToHash("Move");

    public GameObject art;
    private static readonly int Direction = Animator.StringToHash("Direction");

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
                animator.SetTrigger(Stun);
                break;
            case StateEnum.ShortAttack:
                animator.SetTrigger(Lunge);
                break;
            case StateEnum.MidAttack:
                animator.SetTrigger(Swing);
                break;
            case StateEnum.LongAttack:
                animator.SetTrigger(Walk);
                break;
            case StateEnum.Chase:
                animator.SetTrigger(Move);
                break;
            case StateEnum.Search:
                animator.SetTrigger(Run);
                break;
            default:
                Debug.LogWarning("No animation mapped for state: " + state);
                break;
        }
    }

    private void Update()
    {
        animator.SetFloat(Direction, transform.rotation.z < 0 ? 0 : 1);
        art.transform.rotation=quaternion.identity;
    }
}
