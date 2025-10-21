using System.Collections.Generic;
using Entities.StateMachine;
using UnityEngine;

namespace Enemies.Valeria.States
{
    public class ValeriaStateMeleeLock<T> : StatesBase<T>
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

        private float _shortMeleeAttackCd=2.4f;
        private float _shortMeleeTimerCd;
        
        private float _longMeleeAttackCd=5;
        private float _longMeleeTimerCd;
        


        private int _direction;
        private Dictionary<int, float> _directions = new Dictionary<int, float>();
        public ValeriaStateMeleeLock(ISteering steering, Rigidbody2D target, float desiredDistance, float stoppingThreshold, float orbitSpeed, float orbitAngle, float cooldown)
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
            MaintainDistance();
            _animate.PlayStateAnimation(StateEnum.Chase);
            Vector2 dir = _steering.GetDir();
            _look.LookDir(dir);
        }
        private void MaintainDistance()
        {
            Vector2 toPlayer = target.position - _move.Position;
            float dist = toPlayer.magnitude;
            
            _shortMeleeTimerCd += Time.deltaTime;
            if (_shortMeleeTimerCd > _shortMeleeAttackCd)
            {
                _status.SetStatus(StatusEnum.OnMeleeShortCD,true);
                _shortMeleeTimerCd = 0;
            }
            _longMeleeTimerCd += Time.deltaTime;
            if (_longMeleeTimerCd > _longMeleeAttackCd)
            {
                _status.SetStatus(StatusEnum.OnMeleeLongtCD,true);
                _longMeleeTimerCd = 0;
            }
            if (dist > _desiredDistance + _stoppingThreshold || dist < _desiredDistance - _stoppingThreshold)// || _isAttackReady)
            {
                MoveToRing(toPlayer);
            }
            else
            {
                OrbitAroundPlayer(toPlayer);
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
            _shortMeleeTimerCd = 0;
            _longMeleeTimerCd = 0;
            _agent._NVagent.ResetPath(); // clear any current path
            _agent._NVagent.velocity = Vector3.zero; // kill current movement
            _animate.StopStateAnimation(StateEnum.Chase);

        }


    }
}