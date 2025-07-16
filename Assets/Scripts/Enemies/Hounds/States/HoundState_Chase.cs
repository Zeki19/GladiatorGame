using UnityEngine;

public class HoundState_Chase<T> : State_Steering<T>
{
    private Transform _target;
    public Vector2 lastSeenPositionOfTarget;
    public HoundState_Chase(ISteering steering, StObstacleAvoidance avoidStObstacles, Transform self, Transform target) : base(steering, avoidStObstacles, self)
    {
        _target = target;
    }
    public override void Enter()
    {
        base.Enter();
        
        _move.ModifySpeed(2f);
        _animate.PlayStateAnimation(StateEnum.Chase);
    }

    public override void Execute()
    {
        base.Execute();
        lastSeenPositionOfTarget = _target.position;
    }
}
