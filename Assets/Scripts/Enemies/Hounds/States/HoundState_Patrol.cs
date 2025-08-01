using Enemies.Hounds.States;
using UnityEngine;

public class HoundState_Patrol<T> : State_Steering<T>
{
    private readonly MonoBehaviour _mono;
    private readonly float _duration;
    public bool TiredOfPatroling;
    private Coroutine _patrolCoroutine;
    public HoundState_Patrol(ISteering steering, StObstacleAvoidance avoidStObstacles, Transform self, MonoBehaviour monoBehaviour,float duration) : base(steering, avoidStObstacles, self)
    {
        _mono = monoBehaviour;
        _duration = duration;
    }
    public override void Enter()
    {
        base.Enter();
        
        _animate.PlayStateAnimation(StateEnum.Patrol);
        
        _patrolCoroutine = _mono.StartCoroutine(StartPatrol());
    }
    
    public override void Exit()
    {
        TiredOfPatroling = false;
        if (_patrolCoroutine != null)
        {
            _mono.StopCoroutine(_patrolCoroutine);
            _patrolCoroutine = null;
        }
        
        base.Exit();
    }
    
    private System.Collections.IEnumerator StartPatrol()
    {
        yield return new WaitForSeconds(_duration);
        TiredOfPatroling = true;
    }
}
