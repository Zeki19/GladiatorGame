using System;
using UnityEngine;
using Weapons;

namespace Interfaces
{
    public interface IAttack
    {
        void StartAttack(Attack attack,Weapon weapon);
        void ExecuteAttack(Attack attack,Weapon weapon);
        void FinishAttack(Attack attack,Weapon weapon);
        Action OnAttack { get; set; }
    }
}
