using System.Collections;
using UnityEngine;

public class HoundState_Search<T> : State_Steering<T>
{
    private MonoBehaviour _mono;
    public bool Searched { get; private set; } = false;

    public HoundState_Search(ISteering steering, ObstacleAvoidance avoidObstacles, Transform self, MonoBehaviour monoBehaviour) : base(steering, avoidObstacles, self)
    {
        _mono = monoBehaviour;
    }
    
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Search");
        
        Searched = false;
        _look.PlayStateAnimation(StateEnum.Chase);
    }

    public override void Execute()
    {
        var dir = _avoidObstacles.GetDir(_self, _steering.GetDir());
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

