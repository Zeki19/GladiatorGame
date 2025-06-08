using UnityEngine;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStateRunAway<T> : State_FollowPoints<T>
    {
        private readonly MonoBehaviour _mono;
        private Transform _target;
        SpriteRenderer _spriteRenderer;
        public FirstBossStateRunAway(Transform entity, Transform target, MonoBehaviour monoBehaviour, SpriteRenderer spriteRenderer) : base(entity)
        {
            Entity = entity;
            DistanceToPoint = .2f;
            _target = target;

            _mono = monoBehaviour;
            _spriteRenderer = spriteRenderer;
        }
    
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Runaway");
            _move.ModifySpeed(2f);
            _look.PlayStateAnimation(StateEnum.Runaway);
            
            _mono.StartCoroutine(DelayedSetPath());
            _spriteRenderer.color = Color.black;
        }
        
        
        
        private System.Collections.IEnumerator DelayedSetPath()
        {
            yield return null;
            SetPath();
        }
        
        private void SetPath()
        {
            var init = Vector3Int.RoundToInt(Entity.transform.position);
            List<Vector3Int> path = ASTAR.Run(init, IsSatisfied, GetConnections, GetCost, Heuristic);
            //path = ASTAR.CleanPath(path, InView);
           var a = _move as FirstBossModel;
           a.Points = path;
            SetWaypoints(path);
        }
        
        public override void Exit()
        {
            base.Exit();
            _move.ModifySpeed(-2f);
        }
        
        #region Utils

            private float Heuristic(Vector3Int current)
            {
                Vector3 targetPos = Vector3Int.RoundToInt(_target.position);
                float baseHeuristic = Vector3.Distance(current, targetPos);
                
                if (GridManager.PickUpHeu.TryGetValue(current, out float pickupValue))
                {
                    baseHeuristic -= pickupValue;
                }

                return baseHeuristic;
            }

            bool IsSatisfied(Vector3Int curr)
            {
                Vector3 targetPos = Vector3Int.RoundToInt(_target.position);
                return Vector3.Distance(curr, targetPos) <= (DistanceToPoint) && InView(curr, Vector3Int.RoundToInt(targetPos));
            }

            
        #endregion
    }
}