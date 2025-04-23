using System;
using UnityEngine;


namespace Interfaces
{
    public interface IAttack
    {
        float damageModifier { get; set; }
        void Attack();
        void ModifyDamage(float damage);
        Action OnAttack { get; set; }
    }
}
