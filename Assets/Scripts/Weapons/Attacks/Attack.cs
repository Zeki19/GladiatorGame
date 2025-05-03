using UnityEngine;

namespace Weapons.Attacks
{
    public abstract class Attack
    {
        protected Weapon Weapon;
        protected GameObject WeaponGO;
        public abstract void StartAttack(Weapon weapon);
        public abstract void ExecuteAttack(Weapon weapon);
        public abstract void FinishAttack(Weapon weapon);
    }
}
