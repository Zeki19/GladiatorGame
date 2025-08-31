using Entities;
using Entities.StateMachine;
using UnityEngine;

namespace Enemies.Valeria.States
{
    public class ValeriaStateInvisibility<T> : StatesBase<T>
    {
        private Rigidbody2D _player;

        private float _invisibilitySpeed;
        private float _regularSpeed;

        private float _smokeInterval = 2f;     // how often to spawn smoke
        private float _smokeTimer;
        private GameObject _smokePrefab;         
        private Transform _smokeParent;          // optional parent for organization

        public ValeriaStateInvisibility(Rigidbody2D player, float invisibilitySpeed, GameObject smokePrefab, Transform smokeParent = null)
        {
            _player = player;
            _invisibilitySpeed = invisibilitySpeed;
            _smokePrefab = smokePrefab;
            _smokeParent = smokeParent;
        }

        public override void Enter()
        {
            base.Enter();
            _move.Move(Vector2.zero);

            _regularSpeed = _agent._NVagent.speed;

            var look = _look as EntityView;
            look.Sprite.enabled = false;

            _agent._NVagent.speed = _invisibilitySpeed;
            _agent._NVagent.updateRotation = false;
            _agent._NVagent.updateUpAxis = false;

            _smokeTimer = _smokeInterval;
        }

        public override void Execute()
        {
            base.Execute();

            _agent._NVagent.SetDestination(_player.position);


            _smokeTimer -= Time.deltaTime;
            if (_smokeTimer <= 0f)
            {
                SpawnSmoke();
                _smokeTimer = _smokeInterval;
            }
        }

        private void SpawnSmoke()
        {
            if (_smokePrefab == null) return;

            GameObject smoke = Object.Instantiate(_smokePrefab, _move.Position, Quaternion.identity, _smokeParent);

            Object.Destroy(smoke, .5f);
        }

        public override void Exit()
        {
            base.Exit();

            _agent._NVagent.speed = _regularSpeed;

            var look = _look as EntityView;
            look.Sprite.enabled = true;
        }
    }
}
