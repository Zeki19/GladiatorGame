using Entities;
using Entities.StateMachine;
using UnityEngine;

namespace Enemies.Minotaur.States
{
    public class MinotaurStateChase<T> : StatesBase<T>
    {
        private ISteering _steering;
        private float _speedMod;
        private float _stackingSpeed;
        private float _speedModeInterval = 4;
        private float _timer;
        private EnemyModel _model;
        private float _shortMeleeAttackCd=2.4f;
        private float _shortMeleeTimerCd;
        
        private float _longAttackCd=5;
        private float _longTimerCd;
        public MinotaurStateChase(ISteering steering, MinotaurController controller,float ShortAttackCD,float longAttackCd)
        {
            _steering = steering;
            _speedMod = controller.stats.Stack;
            _speedModeInterval = controller.stats.Interval;
            _shortMeleeAttackCd = ShortAttackCD;
            _longAttackCd = longAttackCd;

        }

        public override void Enter()
        {
            base.Enter();
            _move.Move(Vector2.zero);
            _animate.PlayStateAnimation(StateEnum.Chase);
            _timer = _speedModeInterval;
            _agent._NVagent.updateRotation = false;
            _agent._NVagent.updateUpAxis = false;
            _model = _move as EnemyModel;
        }
        public override void Execute()
        {
            base.Execute();
            
            _agent._NVagent.SetDestination(_target.GetTarget().transform.position);
            _animate.PlayStateAnimation(StateEnum.Chase);
            if (Vector3.Distance(_target.GetTarget().transform.position, _model.Position) <
                _agent._NVagent.stoppingDistance)
            {
                _agent._NVagent.velocity = Vector3.zero;
            }
            
            _shortMeleeTimerCd += Time.deltaTime;
            if (_shortMeleeTimerCd > _shortMeleeAttackCd)
            {
                _status.SetStatus(StatusEnum.OnMeleeShortCD,true);
                _shortMeleeTimerCd = 0;
            }
            _longTimerCd += Time.deltaTime;
            if (_longTimerCd > _longAttackCd)
            {
                _status.SetStatus(StatusEnum.OnLongCD,true);
                _longTimerCd = 0;
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
            if (_model.RaycastBetweenCharacters(_model.transform, _target.GetTarget().transform).collider != null)
            {
                Debug.Log("i dont see the player");
                _status.SetStatus(StatusEnum.SawThePlayer, false);
                //Now follow the player wanting to charge (DesperateSearch)
            }
            else
            {
                _status.SetStatus(StatusEnum.SawThePlayer, true);
                //Now follow the player normally (Chase)
            }

            Vector2 dir = _steering.GetDir();
            //_move.Move(dir);
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
