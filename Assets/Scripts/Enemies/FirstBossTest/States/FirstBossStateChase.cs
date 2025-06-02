using UnityEngine;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStateChase<T> : States_Base<StateEnum>
    {
        public override void Enter()
        {
            Debug.Log("Chase");
        }
    }
}