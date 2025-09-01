using Entities.StateMachine;

namespace Enemies.Valeria.States
{
    public class ValeriaStateDash<T> : StatesBase<T>
    {
        public override void Enter()
        {
            _sound.PlaySound("BackStep", "Enemy");
            var dashData = _statesData.GetStateData<DashStateData>(EnemyStates.Dash);
            if (dashData != null)
            {
                _move.Dash(dashData.Direction, dashData.Force, dashData.Distance);
                _status.SetStatus(StatusEnum.Dashing, true);
            }
        }
    }
}