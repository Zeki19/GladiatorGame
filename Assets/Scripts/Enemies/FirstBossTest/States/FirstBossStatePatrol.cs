using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStatePatrol<T> : State_FollowPoints<T>
    {
        private readonly float _duration;
        public bool TiredOfPatroling;
        private Coroutine _patrolCoroutine;
        private FirstBossModel _model;
        public FirstBossStatePatrol(Transform entity, float duration, List<Vector3Int> waypoints, MonoBehaviour monoBehaviour, SpriteRenderer spriteRenderer) : base(entity, monoBehaviour)
        {
            _duration = duration;
            SetWaypoints(waypoints);
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Patrol");

            if (_model == null)
            {
                _model=_move as FirstBossModel;
            }
            _model.isTired = false;
            _look.PlayStateAnimation(StateEnum.Patrol);
            
            _patrolCoroutine = _mono.StartCoroutine(StartPatrol());

        }

        public override void Execute()
        {
            base.Execute();
        }

        protected override void Run()
        {
            if (Waypoints == null || Waypoints.Count == 0) return;

            Vector3 point = Waypoints[_index];
            point.z = Entity.position.z;
            Vector3 dir = point - Entity.position;

            if (dir.magnitude < DistanceToPoint)
            {
                Entity.position = Waypoints[_index];
                
                _index = (_index + 1) % Waypoints.Count;
            }

            OnMove(dir.normalized);
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
            _model.isTired = true;
        }
    }
}