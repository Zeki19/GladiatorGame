using Entities;
using Entities.StateMachine;
using UnityEngine;

namespace Enemies.Gaius.States
{
    public class GaiusStateIdle<T> : StatesBase<T>
    {
        private GaiusStatsSO _stats;
        private float _idleTime = 1f;

        public override void Enter()
        {
            base.Enter();
            _sound.PlaySound("Cooldown", "Enemy");
            _animate.PlayStateAnimation(StateEnum.Idle);
            _move.Move(Vector2.zero);
            _look.LookDir(Vector2.zero);
        }

        public override void Execute()
        {
            base.Execute();
            _idleTime -= Time.deltaTime;
            _look.LookDir(((Vector2)_target.GetTarget().transform.position-_move.Position).normalized);
            if (_idleTime <= 0.0f)
            {
                _status.SetStatus(StatusEnum.Dashing,false);
                _status.SetStatus(StatusEnum.Attacking,false);
                _status.SetStatus(StatusEnum.AttackMissed,false);
            }
        }
    }
}