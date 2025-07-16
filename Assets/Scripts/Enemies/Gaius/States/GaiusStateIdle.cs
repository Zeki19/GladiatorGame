using Entities;
using Entities.StateMachine;
using UnityEngine;

namespace Enemies.Gaius.States
{
    public class GaiusStateIdle<T> : StatesBase<T>
    {
        private GaiusController _gaiusController;
        private GaiusStatsSO _stats;
        private float _idleTime;
        public GaiusStateIdle(GaiusController GaiusController)
        {
            _gaiusController = GaiusController;
            _stats = GaiusController.stats;
            _idleTime = 1f;

        }

        public override void Enter()
        {
            base.Enter();
            _sound.PlaySound("Cooldown", "Enemy");
            _animate.PlayStateAnimation(StateEnum.Idle);
            switch (StateMachine.LastStateEnum())
            {
                case StateEnum.ShortAttack:
                    _idleTime = _stats.shortPunish;
                    break;

                case StateEnum.MidAttack:
                    _idleTime = _stats.mediumPunish;
                    break;

                case StateEnum.LongAttack:
                    _idleTime = _stats.longPunish;
                    break;

                default:
                    Debug.LogError("The idle case you are trying to acces is not contemplated.");
                    break;
            }
            _move.Move(Vector2.zero);
            _look.LookDir(Vector2.zero);
        }

        public override void Execute()
        {
            base.Execute();
            _idleTime -= Time.deltaTime;
            if (_idleTime <= 0.0f)
            {
                _gaiusController.isBackStepFinished = false;
                _gaiusController.FinishedAttacking = false;
                _gaiusController.didAttackMiss = false;
            }
        }
    }
}