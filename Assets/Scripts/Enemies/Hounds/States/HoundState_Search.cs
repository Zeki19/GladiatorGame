using System.Collections;
using UnityEngine;

public class HoundState_Search<T> : State_Steering<T>
{
    private MonoBehaviour _mono;
    public bool Searched { get; private set; } = false;

    public HoundState_Search(ISteering steering, StObstacleAvoidance avoidStObstacles, Transform self, MonoBehaviour monoBehaviour) : base(steering, avoidStObstacles, self)
    {
        _mono = monoBehaviour;
    }
    
    public override void Enter()
    {
        base.Enter();
        
        Searched = false;
        _animate.PlayStateAnimation(StateEnum.Chase);
    }

    public override void Execute()
    {
        var dir = AvoidStObstacles.GetDir(_self, _steering.GetDir());
        if (dir == Vector2.zero)
        {
            _look.LookDir(Vector2.right);
            _mono.StartCoroutine(Timer(2));
        }
        _move.Move(dir);
        _look.LookDir(dir);
    }

    public void ChangeSteering(ISteering newSteering)
    {
        _steering = newSteering;
    }

    private IEnumerator Timer(float duration)
    {
        yield return new WaitForSeconds(duration);
        Searched = true;
    }


}

