using UnityEngine;
using UnityEngine.Rendering;

public class StatueView : EnemyView, ILook
{
    private StateEnum _lastAnimationState = StateEnum.Idle;
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

    public void LookSpeedMultiplier(float mult)
    {
        throw new System.NotImplementedException();
    }

    public override void PlayStateAnimation(StateEnum state)
    {
        if(state == _lastAnimationState)
            return;
        _lastAnimationState = state;
        switch (state)
        {
            case StateEnum.Idle :animator.SetTrigger("Idle");
                break;
            case StateEnum.Chase :animator.SetTrigger("Chase");
                break;
            case StateEnum.Attack :animator.SetTrigger("Attack");
                break;
            default:
                break;
        }
    }
}
