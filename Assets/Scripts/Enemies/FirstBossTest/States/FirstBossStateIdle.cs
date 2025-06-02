using UnityEngine;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStateIdle<T> : States_Base<StateEnum>
    {
        public override void Enter()
        {
            Debug.Log("IDLE");
        }
    }
}