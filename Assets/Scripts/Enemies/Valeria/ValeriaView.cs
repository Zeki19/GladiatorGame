using System;
using Enemies;
using Entities.Interfaces;
using Unity.Mathematics;
using UnityEngine;

public class ValeriaView : EnemyView, ILook
{
    private static readonly int Direction = Animator.StringToHash("Direction");
    private static readonly int ForwardDash = Animator.StringToHash("ForwardDash");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int BackDash = Animator.StringToHash("BackDash");
    private static readonly int ShadowDash = Animator.StringToHash("ShadowDash");
    private static readonly int FrontalAttack = Animator.StringToHash("FrontalAttack");
    private static readonly int DoubleAttack = Animator.StringToHash("DoubleAttack");
    private static readonly int DaggerThrow = Animator.StringToHash("DaggerThrow");
    private static readonly int Phase = Animator.StringToHash("Phase");
    public GameObject art;
    public SpriteRenderer shadowSprite;
    private void Update()
    {
        animator.SetFloat(Direction, transform.rotation.z < 0 ? 1 : 0.1f);
        animator.SetFloat(Phase, manager.HealthComponent.currentHealth > manager.phasesThresholds[0]
            ? 0.1f
            : 1);
        art.transform.rotation = quaternion.identity;
    }

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
            case StateEnum.Chase:
                animator.SetTrigger(Move);
                break;
            case StateEnum.Dash:
                animator.SetTrigger(ForwardDash);
                break;
            case StateEnum.BackStep:
                animator.SetTrigger(BackDash);
                break;
            case StateEnum.ShadowDash:
                animator.SetTrigger(ShadowDash);
                break;
            case StateEnum.ShortAttack:
                animator.SetTrigger(FrontalAttack);
                break;
            case StateEnum.MidAttack:
                animator.SetTrigger(DoubleAttack);
                break;
            case StateEnum.LongAttack:
                animator.SetTrigger(DaggerThrow);
                break;
            case StateEnum.Death:
                animator.SetTrigger("Death");
                break;
            case StateEnum.Win:
                animator.SetTrigger("Win");
                break;
            default:
                Debug.LogWarning("No animation mapped for state: " + state);
                break;
        }
    }
    public override void StopStateAnimation(StateEnum state)
    {
        switch (state)
        {
            case StateEnum.Idle:
                animator.ResetTrigger(Idle);
                break;
            case StateEnum.Chase:
                animator.ResetTrigger(Move);
                break;
            case StateEnum.Dash:
                animator.ResetTrigger(ForwardDash);
                break;
            case StateEnum.BackStep:
                animator.ResetTrigger(BackDash);
                break;
            case StateEnum.ShadowDash:
                animator.ResetTrigger(ShadowDash);
                break;
            case StateEnum.ShortAttack:
                animator.ResetTrigger(FrontalAttack);
                break;
            case StateEnum.MidAttack:
                animator.ResetTrigger(DoubleAttack);
                break;
            case StateEnum.LongAttack:
                animator.ResetTrigger(DaggerThrow);
                break;
            default:
                Debug.LogWarning("No animation mapped for state: " + state);
                break;
        }
    }

    public override void LookDirInsta(Vector2 dir)
    {
        float angle = Vector2.SignedAngle(Vector2.up, dir);
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        transform.rotation = targetRotation;
    }
}
