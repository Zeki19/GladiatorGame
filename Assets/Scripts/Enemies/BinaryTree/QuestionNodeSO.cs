using System;
using System.Collections.Generic;
using Enemies.BinaryTree.QuestionFunctions;
using Enemies.FirstBossTest;
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
                { QuestionEnum.IsInIdleState, IsInIdleState },
                { QuestionEnum.IsInChaseState, IsInChaseState },
                { QuestionEnum.IsInPatrolState, IsInPatrolState },
                { QuestionEnum.IsInSearchState, IsInSearchState },
                { QuestionEnum.IsInAttackState, IsInAttackState },
                { QuestionEnum.IsPlayerAlive, IsPlayerAlive },
                { QuestionEnum.IsRested, IsRested },
                { QuestionEnum.IsTired, IsTired },
                { QuestionEnum.IsAttackOnCd, IsAttackOnCd },
                { QuestionEnum.FinishedSearching, FinishedSearching },
                { QuestionEnum.WasLastStateAttack, WasLastStateAttack },
                { QuestionEnum.DidAttackMiss, DidAttackMiss },
                { QuestionEnum.IsInShortRange, c => PlayerInAttackRange(c, 0) },
                { QuestionEnum.IsInMidRange, c => PlayerInAttackRange(c, 1) },
                { QuestionEnum.IsInLongRange, c => PlayerInAttackRange(c, 2) },
                { QuestionEnum.IsInPhase1, IsInPhase1}
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
            var model = arg.model as FirstBossModel;
            return model != null && model.isSearchFinish;
        }
        private bool IsAttackOnCd(AIContext arg)
        {
            var model = arg.model as FirstBossModel;
            return model != null && model.isAttackOnCd;
        }
        private bool IsTired(AIContext arg)
        {
            var model = arg.model as FirstBossModel;
            return model != null && model.isTired;
        }
        private bool IsRested(AIContext arg)
        {
            var model = arg.model as FirstBossModel;
            return model != null && model.isRested;
        }
        private bool IsPlayerAlive(AIContext arg)
        {
            return true;
        }
        private bool IsInIdleState(AIContext arg)
        {
            //var controller = arg.controller as FirstBossController;
            //return arg.stateMachine.CurrentState() == controller?.IdleState;
            var b = arg.stateMachine.CurrentStateEnum() == StateEnum.Idle;
            return b;
        }
        private bool IsInChaseState(AIContext arg)
        {
            //var controller = arg.controller as FirstBossController;
            //return arg.stateMachine.CurrentState() == controller?.ChaseState;
            return arg.stateMachine.CurrentStateEnum() == StateEnum.Chase;
        }
        private bool IsInPatrolState(AIContext arg)
        {
            //var controller = arg.controller as FirstBossController;
            //return arg.stateMachine.CurrentState() == controller?.PatrolState;
            var b = arg.stateMachine.CurrentStateEnum() == StateEnum.Patrol;
            return b;
        }
        private bool IsInSearchState(AIContext arg)
        {
            //var controller = arg.controller as FirstBossController;
            //return arg.stateMachine.CurrentState() == controller?.SearchState;
            return arg.stateMachine.CurrentStateEnum() == StateEnum.Search;
        }
        private bool IsInAttackState(AIContext arg)
        {
            return arg.stateMachine.CurrentStateEnum() == StateEnum.Attack;
        }
        private bool WasLastStateAttack(AIContext arg)
        {
            var lastState = arg.stateMachine.LastStateEnum();
            return lastState == StateEnum.ShortAttack || lastState == StateEnum.MidAttack || lastState == StateEnum.LongAttack;
        }
        private bool DidAttackMiss(AIContext arg)     
        {
            var controller = arg.controller as GaiusController;
            return controller != null && controller.didAttackMiss;
        }
        
        /*
        private bool IsInShortRange(AIContext arg)    
        {
            var controller = arg.controller as GaiusController;
            return Vector3.Distance(arg.selfGameObject.transform.position, arg.playerGameObject.transform.position) <= controller.shortRange;
        }
        private bool IsInMidRange(AIContext arg)      
        {
            var controller = arg.controller as GaiusController;
            return Vector3.Distance(arg.selfGameObject.transform.position, arg.playerGameObject.transform.position) <= controller.midRange;
        }
        private bool IsInLongRange(AIContext arg)     
        {
            var controller = arg.controller as GaiusController;
            return Vector3.Distance(arg.selfGameObject.transform.position, arg.playerGameObject.transform.position) <= controller.longRange;
        }
        private bool IsWithinDistance(AIContext arg, float range)
        {
            Vector3 posA = arg.selfGameObject.transform.position;
            Vector3 posB = arg.playerGameObject.transform.position;
            return Vector3.Distance(posA, posB) <= range;
        }
        */
        
        private bool IsInPhase1(AIContext arg) 
        {
            return arg.controller.CurrentPhase == 1;
        }

    }
}