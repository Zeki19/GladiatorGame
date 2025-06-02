using UnityEngine;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStatePatrol<T> : States_Base<StateEnum>
    {
        public override void Enter()
        {
            Debug.Log("Patrol");
        }
    }
}