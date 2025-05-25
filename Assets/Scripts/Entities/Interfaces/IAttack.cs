using System;
using Weapons;
using Weapons.Attacks;

namespace Entities.Interfaces
{
    public interface IAttack
    {
        void StartAttack(Attack attack,Weapon weapon);
        void ExecuteAttack(Attack attack,Weapon weapon);
        void FinishAttack(Attack attack,Weapon weapon);
        Action OnAttack { get; set; }
    }
}
