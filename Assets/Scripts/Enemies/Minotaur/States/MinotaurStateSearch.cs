using Entities;
using Entities.StateMachine;
using UnityEngine;

namespace Enemies.Minotaur.States
{
    public class MinotaurStateSearch<T> : StatesBase<T>
    {
        private ISteering _steering;
        MinotaurModel _model;
        private float _timer;
        private float _speedMod;
        private float _stackingSpeed;
        private float _speedModeInterval = 4;
        public MinotaurStateSearch(ISteering steering,MinotaurController controller)
        {
            _steering = steering;
            _speedMod = controller.stats.Stack;
            _speedModeInterval = controller.stats.Interval;
        }

        public override void Enter()
        {
            base.Enter();
            _move.Move(Vector2.zero);
            _animate.PlayStateAnimation(StateEnum.Chase);
            _timer = _speedModeInterval;
            _agent._NVagent.updateRotation = false;
            _agent._NVagent.updateUpAxis = false;
            _agent._NVagent.SetDestination(_target.GetTarget().transform.position);
            _model = _move as MinotaurModel;
        }
        public override void Execute()
        {
            base.Execute();
            _animate.PlayStateAnimation(StateEnum.Chase);
            if (_model.RaycastBetweenCharacters(_model.transform, _target.GetTarget().transform).collider != null)
            {
                _status.SetStatus(StatusEnum.SawThePlayer, false);
            }
            else
            {
                _status.SetStatus(StatusEnum.SawThePlayer, true);
                _status.SetStatus(StatusEnum.FinishedSearching, true);
                //Now follow the player normally (Chase)
            }
            _timer -= Time.deltaTime;
            if(_timer<0)
            {
                if(_agent._NVagent.speed<=12)
                {
                    _agent._NVagent.speed += _speedMod;
                    _stackingSpeed += _speedMod;
                }
                _timer = _speedModeInterval;
            }
            
            if (_agent._NVagent.remainingDistance <= 1&&_agent._NVagent.hasPath)
            {
                _status.SetStatus(StatusEnum.FinishedSearching, true);
                if(_agent._NVagent.speed-_stackingSpeed<=10)
                {
                    _agent._NVagent.speed += _speedMod;
                    Debug.Log("aa");
                }
            }
            if (_agent._NVagent.remainingDistance == 0&&!_agent._NVagent.hasPath)
            {
                _agent._NVagent.SetDestination(_target.GetTarget().transform.position);
            }
            Vector2 dir = _steering.GetDir();
            _look.LookDir(dir);
        }
        public override void Exit()
        {
            base.Exit();
            _animate.StopStateAnimation(StateEnum.Chase);
            _agent._NVagent.speed -= _stackingSpeed;
            _stackingSpeed = 0;
            _agent._NVagent.ResetPath(); // clear any current path
            _agent._NVagent.velocity = Vector3.zero; // kill current movement
        }
    }
}
