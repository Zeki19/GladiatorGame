using Entities;
using Entities.StateMachine;

public class GaiusStateDash<T> : StatesBase<T>
{
    private EntityManager _manager;
    public GaiusStateDash(EntityManager manager) 
    {
        _manager = manager;
    }
    public override void Enter()
    {
        _sound.PlaySound("BackStep", "Enemy");
        _manager.PlaySound("BackStep");
        _agent.TurnOffNavMesh();
        var dashData = _statesData.GetStateData<DashStateData>(EnemyStates.Dash);
        if (dashData != null)
        {
            _move.Dash(dashData.Direction, dashData.Force, dashData.Distance);
            _status.SetStatus(StatusEnum.Dashing, true);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _agent.RepositionInNavMesh();
    }
}