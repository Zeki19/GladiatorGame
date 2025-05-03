using System;
using UnityEngine;
using Weapons;
using Weapons.Attacks;

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
