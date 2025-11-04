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

        private float _smokeInterval = 2f;
        private float _smokeTimer;
        private GameObject _smokePrefab;
        private Transform _smokeParent;

        private bool _isOrbiting;
        private float _orbitRadius = 1.5f;
        private float _orbitDuration = 2f;
        private float _elapsedTime;

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
            _animate.PlayStateAnimation(StateEnum.Chase);

            _regularSpeed = _agent._NVagent.speed;

            var look = _look as ValeriaView;
            look.Sprite.enabled = false;
            look.shadowSprite.enabled = false;

            _agent._NVagent.speed = _invisibilitySpeed;
            _agent._NVagent.updateRotation = false;
            _agent._NVagent.updateUpAxis = false;

            _smokeTimer = _smokeInterval;
            _isOrbiting = false;
        }

        public override void Execute()
        {
            base.Execute();
            Vector2 toPlayer = _player.position - _move.Position;
            if (!_isOrbiting)
            {
                _agent._NVagent.SetDestination(_player.position);

                if (Vector2.Distance(_move.Position, _player.position) < _orbitRadius * 1.5f)
                {
                    _elapsedTime = 0f;
                    _isOrbiting = true;
                }
            }
            else
            {
                OrbitAroundPlayer(toPlayer);
            }

            _smokeTimer -= Time.deltaTime;
            if (_smokeTimer <= 0f)
            {
                SpawnSmoke();
                _smokeTimer = _smokeInterval;
            }
        }

        private void OrbitAroundPlayer(Vector2 toPlayer)
        {
            _elapsedTime += Time.deltaTime;

            Vector2 ringPoint = _player.position - toPlayer.normalized * _orbitRadius;
            Vector2 perp = new Vector2(-toPlayer.y, toPlayer.x).normalized;
            Vector2 orbitTarget = ringPoint + perp;
            _agent._NVagent.SetDestination(orbitTarget);

            if (_elapsedTime >= _orbitDuration)
            {
                _isOrbiting = false;
                _status.SetStatus(StatusEnum.isInBack, true);
            }
        }

        private void SpawnSmoke()
        {
            if (_smokePrefab == null) return;

            GameObject smoke = Object.Instantiate(_smokePrefab, _move.Position, Quaternion.identity, _smokeParent);
            Object.Destroy(smoke, 0.5f);
        }

        public override void Exit()
        {
            base.Exit();

            _agent._NVagent.speed = _regularSpeed;

            var look = _look as EntityView;
            look.Sprite.enabled = true;

            _agent._NVagent.ResetPath(); // clear any current path
            _agent._NVagent.velocity = Vector3.zero; // kill current movement
        }
    }
}
