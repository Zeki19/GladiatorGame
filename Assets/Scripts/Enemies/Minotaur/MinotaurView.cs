using System;
using Enemies;
using Entities.Interfaces;
using Unity.Mathematics;
using UnityEngine;

public class MinotaurView : EnemyView, ILook
{
    private static readonly int Direction = Animator.StringToHash("Direction");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int BoulderThrow = Animator.StringToHash("BoulderThrow");
    private static readonly int BullCharge = Animator.StringToHash("BullCharge");
    private static readonly int GroundSlam = Animator.StringToHash("GroundSlam");
    private static readonly int TriHitCombo = Animator.StringToHash("TriHitCombo");
    private static readonly int Phase = Animator.StringToHash("Phase");
    public GameObject art;

    private void Update()
    {
        animator.SetFloat(Direction, transform.rotation.z < 0 ? 0.1f : 1);
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
            case StateEnum.LongAttack:
                animator.SetTrigger(BoulderThrow);
                break;
            case StateEnum.ChargeAttack:
                animator.SetTrigger(BullCharge);
                break;
            case StateEnum.ShortAttack:
                animator.SetTrigger(GroundSlam);
                break;
            case StateEnum.MidAttack:
                animator.SetTrigger(TriHitCombo);
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
            case StateEnum.LongAttack:
                animator.ResetTrigger(BoulderThrow);
                break;
            case StateEnum.ChargeAttack:
                animator.ResetTrigger(BullCharge);
                break;
            case StateEnum.ShortAttack:
                animator.ResetTrigger(GroundSlam);
                break;
            case StateEnum.MidAttack:
                animator.ResetTrigger(TriHitCombo);
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
