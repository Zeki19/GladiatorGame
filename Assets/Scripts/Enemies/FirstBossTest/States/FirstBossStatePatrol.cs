using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStatePatrol<T> : State_FollowPoints<T>
    {
        private readonly float _duration;
        public bool TiredOfPatroling;
        private Coroutine _patrolCoroutine;
        private FirstBossModel _model;
        SpriteRenderer _spriteRenderer;
        public FirstBossStatePatrol(Transform entity, float duration, List<Vector3Int> waypoints, MonoBehaviour monoBehaviour, SpriteRenderer spriteRenderer) : base(entity, monoBehaviour)
        {
            _duration = duration;
            _spriteRenderer = spriteRenderer;
            Waypoints = waypoints;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Patrol");
            
            if (_model==null) _model=_move as FirstBossModel;

            _look.PlayStateAnimation(StateEnum.Patrol);
            
            
            _patrolCoroutine = _mono.StartCoroutine(StartPatrol());

            _spriteRenderer.color = Color.green;
        }
        
        public override void Exit()
        {
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