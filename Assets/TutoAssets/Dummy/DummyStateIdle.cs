using Entities;
using Entities.StateMachine;
using UnityEngine;

namespace Enemies.Dummy.States
{
    public class DummyStateIdle<T> : StatesBase<T>
    {
        private float _idleTime = 1f;

        public override void Enter()
        {
            base.Enter();
            _sound?.PlaySound("Idle", "Dummy");
            _animate?.PlayStateAnimation(StateEnum.Idle);
            _move?.Move(Vector2.zero); // Asegura que no se mueva
            _look?.LookDir(Vector2.zero); // No mira a ningún lado específico
        }

        public override void Execute()
        {
            base.Execute();
            _idleTime -= Time.deltaTime;

            // El dummy siempre resetea sus status pero nunca cambia de estado
            if (_idleTime <= 0.0f)
            {
                _status?.SetStatus(StatusEnum.Dashing, false);
                _status?.SetStatus(StatusEnum.Attacking, false);
                _status?.SetStatus(StatusEnum.AttackMissed, false);

                // Reinicia el timer para seguir en idle
                _idleTime = 1f;
            }

            // Asegura que siempre esté quieto
            _move?.Move(Vector2.zero);
        }

        public override void Exit()
        {
            base.Exit();
            // El dummy nunca debería salir del idle, pero por consistencia
            _animate?.StopStateAnimation(StateEnum.Idle);
        }
    }
}