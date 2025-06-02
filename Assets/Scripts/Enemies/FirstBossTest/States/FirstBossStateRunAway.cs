using UnityEngine;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStateRunAway<T> : States_Base<StateEnum>
    {
        public override void Enter()
        {
            Debug.Log("RunAway");
        }
    }
}