using Entities.StateMachine;
using UnityEngine;

public class HoundState_Idle<T> : StatesBase<T>
{
    private readonly MonoBehaviour _mono;
    private readonly float _duration;
    public bool FinishedResting;

    public HoundState_Idle(MonoBehaviour monoBehaviour, float duration)
    {
        _mono = monoBehaviour;
        _duration = duration;
    }

    public override void Enter()
    {
        base.Enter();
        
        _move.Move(Vector2.zero);
        _animate.PlayStateAnimation(StateEnum.Idle);
        
        _mono.StartCoroutine(StartResting());
    }

    public override void Exit()
    {
        FinishedResting = false;
        base.Exit();
    }
    
    private System.Collections.IEnumerator StartResting()
    {
        yield return new WaitForSeconds(_duration);
        FinishedResting = true;
    }
}
