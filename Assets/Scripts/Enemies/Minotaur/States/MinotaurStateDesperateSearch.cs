using Entities;
using Entities.StateMachine;
using UnityEngine;

namespace Enemies.Minotaur.States
{
    public class MinotaurStateDesperateSearch<T> : StatesBase<T>
    {
        private ISteering _steering;
        private float _speedMod;
        MinotaurModel _model;
        public MinotaurStateDesperateSearch(ISteering steering, float speedMod)
        {
            _steering = steering;
            _speedMod = speedMod;
            
        }

        public override void Enter()
        {
            base.Enter();
            _move.Move(Vector2.zero);
            _animate.PlayStateAnimation(StateEnum.Chase);
            _agent._NVagent.speed += _speedMod;
            _agent._NVagent.updateRotation = false;
            _agent._NVagent.updateUpAxis = false;
            _model = _move as MinotaurModel;
        }
        public override void Execute()
        {
            base.Execute();
            _animate.PlayStateAnimation(StateEnum.Chase);
            _agent._NVagent.SetDestination(_target.GetTarget().transform.position);
            if (_model.RaycastBetweenCharacters(_model.transform, _target.GetTarget().transform).collider == null)
            {
                _status.SetStatus(StatusEnum.ChargeNow, true);
                Debug.Log("ChargeNow");
                //Perform LabyrinthCharge attack
            }
            Vector2 dir = _steering.GetDir();
            _look.LookDir(dir);
        }
        public override void Exit()
        {
            base.Exit();
            _animate.StopStateAnimation(StateEnum.Chase);
            _agent._NVagent.speed -= _speedMod;
            _agent._NVagent.ResetPath(); // clear any current path
            _agent._NVagent.velocity = Vector3.zero; // kill current movement
        }
    }
}
