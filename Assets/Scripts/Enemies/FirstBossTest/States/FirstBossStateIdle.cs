using UnityEngine;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStateIdle<T> : States_Base<T>
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
            if (_model==null) _model=_move as FirstBossModel;
            _move.Move(Vector2.zero);
            _look.PlayStateAnimation(StateEnum.Idle);
            _mono.StartCoroutine(StartResting());
        }

        public override void Exit()
        {
            
            base.Exit();
        }
    
        private System.Collections.IEnumerator StartResting()
        {
            yield return new WaitForSeconds(_duration);
            _model.isRested = true;
        }
    }
}