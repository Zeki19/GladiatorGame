using Entities;
using UnityEngine;

namespace Enemies.Gaius.States
{
    public class GaiusStateIdle<T> : States_Base<T>
    {
        private SpriteRenderer _spriteRenderer;
        private GaiusController _gaiusController;
        private GaiusStatsSO _stats;
        private float _idleTime;
        private EntityManager _gaiusManager;
        public GaiusStateIdle(SpriteRenderer spriteRenderer, GaiusController GaiusController,EntityManager manager)
        {
            _spriteRenderer = spriteRenderer;
            _gaiusController = GaiusController;
            _stats = GaiusController.stats;
            _idleTime = 1f;
            _gaiusManager = manager;

        }

        public override void Enter()
        {
            base.Enter();
            _gaiusManager.view.PlayStateAnimation(StateEnum.Idle);

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
            _spriteRenderer.color = Color.blue;
            _gaiusManager.model.Move(Vector2.zero);
            _gaiusManager.view.LookDir(Vector2.zero);
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