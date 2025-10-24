using Entities.StateMachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemies.Valeria.States
{
    public class ValeriaStateRunAway<T> : StatesBase<T>
    {
        private Rigidbody2D _player;
        private float _searchRadius = 20f;
        private float _hideOffset = 1.5f; // distance behind object
        private LayerMask _hiddingLayer;


        private float _waitTime;       // how long to wait at the spot
        private float _timer;               // countdown
        private bool _waiting;              // is currently waiting

        public Vector2 currentDestination;
        public ValeriaStateRunAway(Rigidbody2D player, LayerMask hiddingLayer, float waitTime)
        {
            _player = player;
            _hiddingLayer = hiddingLayer;
            _waitTime = waitTime;
            _waiting = false;
        }

        public override void Enter()
        {
            base.Enter();
            _move.Move(Vector2.zero);
            _animate.PlayStateAnimation(StateEnum.Chase);

            _agent._NVagent.updateRotation = false;
            _agent._NVagent.updateUpAxis = false;
            //_agent._NVagent.speed *=2 ;

            Vector2 hidePoint = RunAwayFromPlayer();
            if (hidePoint != Vector2.zero) 
            { 
                _agent._NVagent.SetDestination(hidePoint);
                currentDestination = hidePoint;
            }
            _waiting = false;
            _timer = _waitTime;
        }

        public override void Execute()
        {
            base.Execute();

            if (_waiting)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    _waiting = false;
                    Debug.Log("Finished waiting");
                }
            }
            else
            {
                // check if we arrived at destination
                if (!_agent._NVagent.pathPending &&
                    _agent._NVagent.remainingDistance <= _agent._NVagent.stoppingDistance)
                {
                    if (!_agent._NVagent.hasPath || _agent._NVagent.velocity.sqrMagnitude == 0f)
                    {
                        // Start waiting once we’ve arrived
                        _waiting = true;
                        _timer = _waitTime;
                        _status.SetStatus(StatusEnum.IsFleeing,false);
                    }
                }
            }
        }

        private Vector2 RunAwayFromPlayer()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(_move.Position, _searchRadius, _hiddingLayer);
            if (hits.Length == 0) return Vector2.zero;

            Collider2D farthest = hits
                .OrderByDescending(h => Vector2.Distance(_player.position, h.transform.position))
                .First();

            Vector2 objPos = farthest.transform.position;
            Vector2 dir = (objPos - _player.position).normalized;
            Vector2 hidePoint = objPos + dir * _hideOffset;

            return hidePoint;
        }

        public override void Exit()
        {
            base.Exit();

            _agent._NVagent.ResetPath(); // clear any current path
            _agent._NVagent.velocity = Vector3.zero; // kill current movement
            
            _animate.StopStateAnimation(StateEnum.Chase);
        }
    }
}
