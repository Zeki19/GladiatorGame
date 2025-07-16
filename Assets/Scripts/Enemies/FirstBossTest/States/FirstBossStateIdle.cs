using Entities.StateMachine;
using UnityEngine;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStateIdle<T> : StatesBase<T>
    {
        private readonly MonoBehaviour _mono;
        private readonly float _duration;
        public bool FinishedResting;
        private FirstBossModel _model;
        public FirstBossStateIdle(MonoBehaviour monoBehaviour, float duration, SpriteRenderer spriteRenderer)
        {
            _mono = monoBehaviour;
            _duration = duration;
        }

        public override void Enter()
        {
            base.Enter();
            if (_model==null) _model = _move as FirstBossModel;
            _move.Move(Vector2.zero);
            _animate.PlayStateAnimation(StateEnum.Idle);
            _mono.StartCoroutine(StartResting());
        }
    
        private System.Collections.IEnumerator StartResting()
        {
            yield return new WaitForSeconds(_duration);
            _model.isRested = true;
        }
    }
}