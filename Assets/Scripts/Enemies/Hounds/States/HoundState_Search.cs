using System.Collections;
using UnityEngine;

public class HoundState_Search<T> : States_Base<T>
{
    private ISteering _steering;
    private ObstacleAvoidance _avoidWalls;
    private MonoBehaviour _self;
    public bool Searched = false;

    private bool coRunning = false;
    
    public HoundState_Search(ISteering steering, ObstacleAvoidance avoidWalls, MonoBehaviour self)
    {
        _steering = steering;
        _self = self;
        _avoidWalls = avoidWalls;
    }

    public override void Enter()
    {
        base.Enter();
        Searched = false;
        
        _self.StartCoroutine(Timer());
        _look.PlayStateAnimation(StateEnum.Chase);
    }
    public override void Execute()
    {
        base.Execute();
        var dir = _avoidWalls.GetDir(_self.transform, _steering.GetDir());
        
        if (dir == Vector2.zero)
        {
            if (coRunning) return;
            coRunning = true;
            
            _self.StartCoroutine(TurnAroundCoroutine());
            return;
        }
        
        _move.Move(dir.normalized);
        _look.LookDir(dir.normalized);
    }
    
    public void ChangeSteering(ISteering newSteering)
    {
        _steering = newSteering;
    }

    private IEnumerator TurnAroundCoroutine()
    {
        yield return new WaitForSeconds(1f);
        
        Searched = true;
        coRunning = false;
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(3f);
        Searched = true;
    }
}

