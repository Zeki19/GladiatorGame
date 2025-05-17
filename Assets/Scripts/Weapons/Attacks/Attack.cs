using System;
using UnityEngine;

namespace Weapons.Attacks
{
    public abstract class Attack
    {
        protected GameObject WeaponGameObject;
        protected Vector3 StartingPosition;
        protected Weapon Weapon;
        public Action FinishAnimation;
        public abstract void StartAttack(Weapon weapon);
        public abstract void ExecuteAttack(Weapon weapon);
        public abstract void FinishAttack(Weapon weapon);
    }
}
