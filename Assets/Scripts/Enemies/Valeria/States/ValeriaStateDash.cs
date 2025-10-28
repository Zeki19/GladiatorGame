using Entities.StateMachine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemies.Valeria.States
{
    public class ValeriaStateDash<T> : StatesBase<T>
    {
        private Coroutine ExitDashTest;
        public override void Enter()
        {
            _sound.PlaySound("Dash", "Enemy");
            _agent.TurnOffNavMesh();
            var dashData = _statesData.GetStateData<DashStateData>(EnemyStates.Dash);
            if (dashData != null)
            {
                _move.Dash(dashData.Direction, dashData.Force, dashData.Distance);
                _status.SetStatus(StatusEnum.Dashing, true);
            }
            var A = _move as EnemyModel;
            ExitDashTest = A.StartCoroutine(ExitDash());
        }

        private IEnumerator ExitDash()
        {
            yield return new WaitForSeconds(2.5f);
            _status.SetStatus(StatusEnum.Dashing, false);

        }

        public override void Exit()
        {
            _agent.RepositionInNavMesh();
            var A = _move as EnemyModel;
            A.StopCoroutine(ExitDashTest);
            base.Exit();
        }
    }
}