using UnityEngine;

namespace Entities.StateMachine
{
    public class DeadState<T> : StatesBase<T>
    {
        public override void Enter()
        {
            base.Enter();
            _animate.PlayStateAnimation(StateEnum.Death);
            _agent._NVagent.velocity = Vector3.zero;
            _agent._NVagent.enabled = false;
        }
    }
}