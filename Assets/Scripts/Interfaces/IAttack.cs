using System;
using UnityEngine;

namespace Interfaces
{
    public interface IAttack
    {
        float AttackRange {  get; }
        void Attack();
        Action OnAttack { get; set; }
    }
}
