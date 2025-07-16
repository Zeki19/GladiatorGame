using System.Collections;
using UnityEngine;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStateSearch<T> : State_Steering<T>
    {
        private MonoBehaviour _mono;
        public bool Searched { get; private set; } = false;
        private FirstBossModel _model;
        public FirstBossStateSearch(ISteering steering, StObstacleAvoidance avoidStObstacles, Transform self, MonoBehaviour monoBehaviour, SpriteRenderer spriteRenderer) : base(steering, avoidStObstacles, self)
        {
            _mono = monoBehaviour;
        }

        public override void Enter()
        {
            base.Enter();
            var updateSearch = _move as FirstBossModel;
            _steering = new StToPoint(updateSearch.lastSeenPlayerPosition, _self);
            if (!_model) _model=_move as FirstBossModel;

            _model.isSearchFinish = false;
            _animate.PlayStateAnimation(StateEnum.Chase);
        }

        public override void Execute()
        {
            var dir = AvoidStObstacles.GetDir(_self, _steering.GetDir());
            if (dir == Vector2.zero||_model.Manager.Rb.linearVelocity.magnitude<=0.1f)
            {
                _look.LookDir(Vector2.right);
                _mono.StartCoroutine(Timer(2));
            }
            _move.Move(dir);
            _look.LookDir(dir);
        }

        public void ChangeSteering(ISteering newSteering)
        {
            _steering = newSteering;
        }

        private IEnumerator Timer(float duration)
        {
            yield return new WaitForSeconds(duration);
            _model.isSearchFinish = true;
        }
    }
}