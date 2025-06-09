using UnityEngine;
using System.Collections.Generic;
using Entities;
using Player;
using Unity.VisualScripting;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStateRunAway<T> : State_FollowPoints<T>
    {
        private Transform _target;
        private EntityManager _entityManager;
        public FirstBossStateRunAway(Transform entity, Transform target, MonoBehaviour monoBehaviour, EntityManager manager, SpriteRenderer spriteRenderer) : base(entity, monoBehaviour)
        {
            Entity = entity;
            DistanceToPoint = .2f;
            _entityManager = manager;
            _target = target;
        }
    
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Runaway");
            _move.ModifySpeed(2f);
            _look.PlayStateAnimation(StateEnum.Runaway);
            _mono.StartCoroutine(DelayedSetPath());
        }
        private System.Collections.IEnumerator DelayedSetPath()
        {
            yield return null;
            SetPath();
        }
        protected override void SetPath()
        {
            var init = Vector3Int.RoundToInt(Entity.transform.position);
            List<Vector3Int> path = ASTAR.Run(init, IsSatisfied, GetConnections, GetCost, Heuristic);
            
            HashSet<Vector3Int> pickUpPositions = new HashSet<Vector3Int>(GridManager.PickUpDictionary.Keys);
            path = ASTAR.CleanPathPickUps(path, InView, pickUpPositions);
            
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

            if (GridManager.PickUpItem.TryGetValue(current, out float pickupValue))
            {
                foreach (var item in GridManager.PickUpDictionary)
                {
                    if (item.Key == current)
                    {
                        PowerUpType type = item.Value;

                        if (type == PowerUpType.Speed)
                        {
                            baseHeuristic -= pickupValue;
                        }
                        else if (type == PowerUpType.Heal)
                        {
                            if (_entityManager.HealthComponent.GetCurrentHealthPercentage() <= 50f) // low HP
                            {
                                baseHeuristic -= pickupValue;
                            }
                        }
                        break;
                    }
                }
            }
            return baseHeuristic;
        }
            private bool IsSatisfied(Vector3Int curr)
            {
                Vector3 targetPos = Vector3Int.RoundToInt(_target.position);
                return Vector3.Distance(curr, targetPos) <= (DistanceToPoint) && InView(curr, Vector3Int.RoundToInt(targetPos));
            }
            private float GetCost(Vector3Int from, Vector3Int child)
            {
                var baseCost = 5f;

                if (GridManager.NextToWall.ContainsKey(child))
                {
                    baseCost += 3f;
                }

                if (GridManager.PickUpItem.TryGetValue(child, out float pickupValue))
                {
                    foreach (var kvp in GridManager.PickUpDictionary)
                    {
                        if (kvp.Key == child)
                        {
                            PowerUpType type = kvp.Value;

                            if (type == PowerUpType.Speed)
                            {

                                baseCost -= pickupValue;
                            }
                            else if (type == PowerUpType.Heal)
                            {
                                if (_entityManager.HealthComponent.GetCurrentHealthPercentage() <= 50f)
                                {
                                    baseCost -= pickupValue;
                                }
                            }

                            break;
                        }
                    }
                }

                return baseCost;
            }
        #endregion
    }
}