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

        private bool _isFlanking;
        private float _flankRadius = 2.5f;
        private float _arcDuration = 1.5f;
        private float _elapsedTime;
        private float _startAngle;
        private float _targetAngle;
        private bool _clockwise = true;

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
            _isFlanking = false;
        }

        public override void Execute()
        {
            base.Execute();

            if (!_isFlanking)
            {
                _agent._NVagent.SetDestination(_player.position);

                if (Vector2.Distance(_move.Position, _player.position) < 3f)
                {
                    StartFlank();
                }
            }
            else
            {
                DoFlank();
            }

            _smokeTimer -= Time.deltaTime;
            if (_smokeTimer <= 0f)
            {
                SpawnSmoke();
                _smokeTimer = _smokeInterval;
            }
        }

        private void StartFlank()
        {
            _isFlanking = true;
            _elapsedTime = 0f;

            Vector2 offset = _move.Position - _player.position;
            _startAngle = Mathf.Atan2(offset.y, offset.x);

            _targetAngle = _startAngle + (_clockwise ? Mathf.PI : -Mathf.PI);
        }

        private void DoFlank()
        {
            _elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(_elapsedTime / _arcDuration);

            float angle = Mathf.Lerp(_startAngle, _targetAngle, t);
            Vector2 playerPos = _player.position;
            Vector2 orbitPos = playerPos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * _flankRadius;

            _agent._NVagent.SetDestination(orbitPos);

            if (t >= 1f)
            {
                _isFlanking = false;
                _status.SetStatus(StatusEnum.isInBack, true);
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
