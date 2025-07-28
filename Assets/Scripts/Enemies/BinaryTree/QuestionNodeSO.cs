using System;
using System.Collections.Generic;
using Enemies.BinaryTree.QuestionFunctions;
using Enemies.Gaius;
using UnityEngine;

namespace Enemies.BinaryTree
{
    [CreateAssetMenu(fileName = "QuestionNodeSO", menuName = "Scriptable Objects/Tree Nodes/QuestionNode")]
    public class QuestionNodeSO : TreeNodeSO
    {
        [SerializeField] public QuestionEnum _question;
        [SerializeField] private TreeNodeSO _tNode;
        [SerializeField] private TreeNodeSO _fNode;
        [SerializeField] private LayerMask _layerMask;
        private Dictionary<QuestionEnum, Func<AIContext, bool>> _questionFunc;

        private void OnEnable()
        {
            _questionFunc = new Dictionary<QuestionEnum, Func<AIContext, bool>>
            {
                { QuestionEnum.PlayerIsInAStraightLine, PlayerIsInAStraightLine },
                { QuestionEnum.IsFarToPoint1, IsFarToPoint1 },
                { QuestionEnum.IsFarToPoint2, IsFarToPoint2 },
                { QuestionEnum.IsNearToPoint1, IsNearToPoint1 },
                {
                    QuestionEnum.IsInIdleState,
                    c => CompareCurrentState(c, new List<EnemyStates>() { EnemyStates.Idle })
                },
                {
                    QuestionEnum.IsInChaseState,
                    c => CompareCurrentState(c, new List<EnemyStates>() { EnemyStates.Chase })
                },
                {
                    QuestionEnum.IsInPatrolState,
                    c => CompareCurrentState(c, new List<EnemyStates>() { EnemyStates.Patrol })
                },
                {
                    QuestionEnum.IsInSearchState,
                    c => CompareCurrentState(c, new List<EnemyStates>() { EnemyStates.Chase })
                },
                {
                    QuestionEnum.IsInAttackState,
                    c => CompareCurrentState(c,
                        new List<EnemyStates>() { EnemyStates.Attack, EnemyStates.Attack, EnemyStates.Attack })
                },
                { QuestionEnum.IsPlayerAlive, IsPlayerAlive },
                { QuestionEnum.IsRested, IsRested },
                { QuestionEnum.IsTired, IsTired },
                { QuestionEnum.IsAttackOnCd, IsAttackOnCd },
                { QuestionEnum.FinishedSearching, FinishedSearching },
                {
                    QuestionEnum.WasLastStateAttack,
                    c => CompareLastState(c,
                        new List<EnemyStates>() { EnemyStates.Attack, EnemyStates.Attack, EnemyStates.Attack })
                },
                { QuestionEnum.DidAttackMiss, DidAttackMiss },
                { QuestionEnum.IsInShortRange, c => PlayerInAttackRange(c, 0) },
                { QuestionEnum.IsInMidRange, c => PlayerInAttackRange(c, 1) },
                { QuestionEnum.IsInLongRange, c => PlayerInAttackRange(c, 2) },
                { QuestionEnum.IsInPhase1, IsInPhase1 },
                { QuestionEnum.IsCurrentlyAttacking, IsCurrentlyAttacking },
                { QuestionEnum.IsJumpingBackwards, IsJumpingBackwards },
                { QuestionEnum.FinishedAttacking, FinishedAttacking },
                { QuestionEnum.CanLongAttack, CanLongAttack }
            };
        }


        public override void Execute(AIContext context)
        {
            if (_questionFunc == null)
                OnEnable();

            if (_questionFunc[_question](context))
            {
                _tNode.Execute(context);
            }
            else
            {
                _fNode.Execute(context);
            }
        }

        private bool PlayerInAttackRange(AIContext context, int attackDistance)
        {
            return Vector3.Distance(context.selfGameObject.transform.position,
                context.playerGameObject.transform.position) <= context.attackRanges[attackDistance];
        }

        private bool PlayerIsInAStraightLine(AIContext context)
        {
            Vector3 origin = context.selfGameObject.transform.position;
            Vector3 direction = (context.playerGameObject.transform.position - origin).normalized;
            float distance = Vector3.Distance(origin, context.playerGameObject.transform.position);
            int layerBit = 1 << context.playerGameObject.layer;
            LayerMask mask = layerBit;
            distance = Mathf.Clamp(distance, 0, 6);
            var hit = Physics2D.Raycast(origin, direction, distance, _layerMask);
            Debug.DrawLine(origin, origin + direction * distance, Color.green);
            if (hit.collider != null)
            {
                return hit.transform == context.playerGameObject.transform;
            }

            return false;
        }

        private bool IsFarToPoint1(AIContext context)
        {
            return Vector2.Distance(context.selfGameObject.transform.position, context.Points[0].Item1) >
                   context.Points[0].Item2;
        }

        private bool IsNearToPoint1(AIContext context)
        {
            return Vector2.Distance(context.selfGameObject.transform.position, context.Points[0].Item1) <
                   context.Points[0].Item2;
        }

        private bool IsFarToPoint2(AIContext context)
        {
            return Vector2.Distance(context.selfGameObject.transform.position, context.Points[1].Item1) >
                   context.Points[1].Item2;
        }

        private bool FinishedSearching(AIContext arg)
        {
            //var model = arg.model as FirstBossModel;
            //return model != null && model.isSearchFinish;
            return false;
        }

        private bool IsAttackOnCd(AIContext arg)
        {
            //var model = arg.model as FirstBossModel;
            //return model != null && model.isAttackOnCd;
            return false;
        }

        private bool IsTired(AIContext arg)
        {
            //var model = arg.model as FirstBossModel;
            //return model != null && model.isTired;
            return false;
        }

        private bool IsRested(AIContext arg)
        {
            //var model = arg.model as FirstBossModel;
            //return model != null && model.isRested;
            return false;
        }

        private bool IsPlayerAlive(AIContext arg)
        {
            return true;
        }

        private bool CompareLastState(AIContext arg, List<EnemyStates> states)
        {
            var lastState = arg.stateMachine.LastStateEnum();
            foreach (var state in states)
            {
                if (lastState == state)
                    return true;
            }

            return false;
        }

        private bool CompareCurrentState(AIContext arg, List<EnemyStates> states)
        {
            var currentState = arg.stateMachine.CurrentStateEnum();
            foreach (var state in states)
            {
                if (currentState == state)
                    return true;
            }

            return false;
        }

        private bool DidAttackMiss(AIContext arg)
        {
            //var controller = arg.controller as GaiusController;
            //return controller != null && controller.didAttackMiss;
            return false;
        }

        private bool IsInPhase1(AIContext arg)
        {
            return arg.controller.CurrentPhase == 1;
        }

        private bool IsCurrentlyAttacking(AIContext arg)
        {
            //var controller = arg.controller as GaiusController;
            //return controller != null && controller.isAttacking;
            return false;
        }

        private bool IsJumpingBackwards(AIContext arg)
        {
            //var controller = arg.controller as GaiusController;
            //return controller != null && controller.isBackStepFinished;
            return false;
        }

        private bool FinishedAttacking(AIContext arg)
        {
            //var controller = arg.controller as GaiusController;
            //return controller != null && controller.FinishedAttacking;
            return false;
        }

        private bool CanLongAttack(AIContext arg)
        {
            //var controller = arg.controller as GaiusController;
            //return controller != null && controller.canLongAttack;
            return false;
        }
    }
}