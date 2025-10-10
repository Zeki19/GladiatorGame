using Entities;
using Entities.StateMachine;
using UnityEngine;

namespace Enemies.Minotaur.States
{
    public class MinotaurStateSearch<T> : StatesBase<T>
    {
        private ISteering _steering;
        MinotaurModel _model;
        public MinotaurStateSearch(ISteering steering)
        {
            _steering = steering;
            
        }

        public override void Enter()
        {
            base.Enter();
            _move.Move(Vector2.zero);
            //_animate.PlayStateAnimation(StateEnum.Chase);
            _agent._NVagent.updateRotation = false;
            _agent._NVagent.updateUpAxis = false;
            _agent._NVagent.SetDestination(_target.GetTarget().transform.position);
            _model = _move as MinotaurModel;
        }
        public override void Execute()
        {
            base.Execute();

            if (_model.RaycastBetweenCharacters(_model.transform, _target.GetTarget().transform).collider != null)
            {
                _status.SetStatus(StatusEnum.SawThePlayer, false);
                //Now follow the player wanting to charge (DesperateSearch)
            }
            else
            {
                _status.SetStatus(StatusEnum.SawThePlayer, true);
                _status.SetStatus(StatusEnum.FinishedSearching, true);
                //Now follow the player normally (Chase)
            }
            
            if (_agent._NVagent.remainingDistance <= 1)
            {
                _status.SetStatus(StatusEnum.FinishedSearching, true);
            }
            Vector2 dir = _steering.GetDir();
            _look.LookDir(dir);
        }
        public override void Exit()
        {
            base.Exit();
            _agent._NVagent.ResetPath(); // clear any current path
            _agent._NVagent.velocity = Vector3.zero; // kill current movement
        }
    }
}
