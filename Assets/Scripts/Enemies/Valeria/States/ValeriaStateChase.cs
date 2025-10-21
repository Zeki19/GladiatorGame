using Entities.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Valeria.States
{
    public class ValeriaStateChase<T> : StatesBase<T>
    {
        private ISteering _steering;
        private Rigidbody2D target;
        private float _desiredDistance;
        private float _stoppingThreshold;
        private float _orbitSpeed;
        private float _orbitAngle;
        private bool _isAttackReady;

        private float _cooldown;
        private float _timer;

        private float _longAttackCd=5;
        private float _longTimerCd;
        
        private float _middleAttackCd=1;
        private float _middleTimerCd;
        
        

        private int _direction;
        private Dictionary<int, float> _directions = new Dictionary<int, float>();
        public ValeriaStateChase(ISteering steering, Rigidbody2D target, float desiredDistance, float stoppingThreshold, float orbitSpeed, float orbitAngle, float cooldown,float longAttackCd=5,float middleAttackCd=1)
        {
            _steering = steering;
            this.target = target;
            _desiredDistance = desiredDistance;
            _stoppingThreshold = stoppingThreshold;
            _orbitSpeed = orbitSpeed;
            _orbitAngle = orbitAngle;
            _directions.Add(1, 50f);
            _directions.Add(-1, 50f);
            _cooldown = cooldown;
            _longAttackCd = longAttackCd;
            _middleAttackCd = middleAttackCd;
        }

        public override void Enter()
        {
            base.Enter();
            _move.Move(Vector2.zero);
            _animate.PlayStateAnimation(StateEnum.Chase);
            _agent._NVagent.updateRotation = false;
            _agent._NVagent.updateUpAxis = false;
            _timer = 0;
            _isAttackReady = false;
            _direction = MyRandom.Roulette(_directions);
        }
        public override void Execute()
        {
            base.Execute();
            _animate.PlayStateAnimation(StateEnum.Chase);
            MaintainDistance();
            Vector2 dir = _steering.GetDir();
            _look.LookDir(dir);
        }
        private void MaintainDistance()
        {
            Vector2 toPlayer = target.position - _move.Position;
            float dist = toPlayer.magnitude;

            if (dist > _desiredDistance + _stoppingThreshold || dist < _desiredDistance - _stoppingThreshold || _isAttackReady)
            {
                MoveToRing(toPlayer);
                _longTimerCd += Time.deltaTime;
                if (_longTimerCd > _longAttackCd)
                {
                    _status.SetStatus(StatusEnum.OnLongCD,true);
                    _longTimerCd = 0;
                }
            }
            else
            {
                OrbitAroundPlayer(toPlayer);
                _middleTimerCd += Time.deltaTime;
                if (_agent._NVagent.velocity.magnitude > .2f)
                {
                    Debug.Log("toSlow");
                    _middleTimerCd += Time.deltaTime;
                }
                if (_middleTimerCd > _middleAttackCd)
                {
                    _status.SetStatus(StatusEnum.OnMiddleCD,true);
                    _middleTimerCd = 0;
                }
            }
        }

        private void MoveToRing(Vector2 toPlayer)
        {
            Vector2 target = this.target.position - toPlayer.normalized * _desiredDistance;
            _agent._NVagent.SetDestination(target);
        }
        private void OrbitAroundPlayer(Vector2 toPlayer)
        {   
            Vector2 ringPoint = target.position - toPlayer.normalized*_desiredDistance;
            Vector2 perp = new Vector2(-toPlayer.y, toPlayer.x).normalized;
            Vector2 orbitTarget = ringPoint + perp * _direction;
            _agent._NVagent.SetDestination(orbitTarget);
            if (_cooldown < _timer)
            {
                _isAttackReady = true;
            }
            else
            {
                _timer += Time.deltaTime; 
            }
        }

        
        public override void Exit()
        {
            base.Exit();
            _agent._NVagent.ResetPath(); // clear any current path
            _agent._NVagent.velocity = Vector3.zero; // kill current movement
            
            _animate.StopStateAnimation(StateEnum.Chase);
        }
    }
}