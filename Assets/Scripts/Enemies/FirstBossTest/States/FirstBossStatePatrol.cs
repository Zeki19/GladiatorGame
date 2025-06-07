using UnityEngine;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStatePatrol<T> : State_Steering<T>
    {
        private readonly MonoBehaviour _mono;
        private readonly float _duration;
        public bool TiredOfPatroling;
        private Coroutine _patrolCoroutine;
        private FirstBossModel _model;

        public FirstBossStatePatrol(ISteering steering, StObstacleAvoidance avoidStObstacles, Transform self,
            MonoBehaviour monoBehaviour, float duration) : base(steering, avoidStObstacles, self)
        {
            _mono = monoBehaviour;
            _duration = duration;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Patrol");
            
            if (_model==null) _model=_move as FirstBossModel;

            _look.PlayStateAnimation(StateEnum.Patrol);

            _patrolCoroutine = _mono.StartCoroutine(StartPatrol());
        }

        public override void Exit()
        {
            //_model.isTired = false;
            if (_patrolCoroutine != null)
            {
                _mono.StopCoroutine(_patrolCoroutine);
                _patrolCoroutine = null;
            }

            base.Exit();
        }

        private System.Collections.IEnumerator StartPatrol()
        {
            yield return new WaitForSeconds(_duration);
            _model.isRested = false;
        }
    }
}