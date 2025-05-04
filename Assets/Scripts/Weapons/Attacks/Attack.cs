using System;
using UnityEngine;

namespace Weapons
{
    public abstract class Attack
    {
        protected Weapon Weapon;
        public Action FinishAnimation;
        public abstract void StartAttack(Weapon weapon);
        public abstract void ExecuteAttack(Weapon weapon);
        public abstract void FinishAttack(Weapon weapon);
    }
}
