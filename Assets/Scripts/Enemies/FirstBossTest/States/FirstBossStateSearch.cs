using UnityEngine;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStateSearch<T> : States_Base<StateEnum>
    {
        public override void Enter()
        {
            Debug.Log("Search");
        }
    }
}