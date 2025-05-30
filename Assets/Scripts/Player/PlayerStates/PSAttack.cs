using UnityEngine;
using Weapons;
using Weapons.Attacks;

namespace Player.PlayerStates
{
    public class PSAttack<T> : PSBase<T>
    {
        protected T InputFinish;
        protected PlayerManager Manager;
        protected Attack CurrentAttack;
        protected Weapon Weapon;

        public PSAttack(T inputFinish, PlayerManager manager)
        {
            InputFinish = inputFinish;
            Manager = manager;
        }

        protected virtual void SetWeapon(Weapon weapon)
        {
            CurrentAttack = weapon.BaseAttack;
            Weapon = weapon;
        }

        public override void Enter()
        {
            SetWeapon(Manager.weapon);
            Weapon.Attacking = true;
            CurrentAttack.FinishAnimation += AttackFinished;
            _attack.StartAttack(CurrentAttack, Weapon);
            _move.ModifySpeed(-Weapon.SlowPercent);
        }

        public override void Execute()
        {
            _attack.ExecuteAttack(CurrentAttack, Weapon);
            _move.Move(Vector2.zero);
        }

        public override void Exit()
        {
            Weapon.Attacking = false;
            Weapon.PutOnCooldown();
            _attack.FinishAttack(CurrentAttack, Weapon);
            CurrentAttack.FinishAnimation -= AttackFinished;
            ServiceLocator.Instance.GetService<PlayerWeaponController>().CheckDurability();
        }

        private void AttackFinished()
        {
            StateMachine.Transition(InputFinish);
            _move.ModifySpeed(Weapon.SlowPercent);
        }
    }
}