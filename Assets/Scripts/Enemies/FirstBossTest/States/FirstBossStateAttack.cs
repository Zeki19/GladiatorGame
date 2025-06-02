using UnityEngine;

namespace Enemies.FirstBossTest.States
{
    internal class FirstBossStateAttack<T> : States_Base<StateEnum>
    {
        public override void Enter()
        {
            Debug.Log("ATTACK");
        }
    }
}