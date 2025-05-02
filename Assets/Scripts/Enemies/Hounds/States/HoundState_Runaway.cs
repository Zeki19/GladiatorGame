using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class HoundState_Runaway<T> : State_Steering<T>
{
    public HoundState_Runaway(ISteering steering, StObstacleAvoidance avoidStObstacles, Transform self) : base(steering, avoidStObstacles, self)
    {

    }
    
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Runaway");
        _move.ModifySpeed(3f);
        _look.PlayStateAnimation(StateEnum.Runaway);
    }
}
