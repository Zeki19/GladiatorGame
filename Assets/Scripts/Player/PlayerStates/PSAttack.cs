using UnityEngine;
using Weapons;
using Weapons.Attacks;

namespace Player.PlayerStates
{
    public class PSAttack<T> : PSBase<T>
    {
        private readonly T _inputFinish;
        private readonly PlayerManager _manager;
        protected Attack CurrentAttack;
        protected Weapon CurrentWeapon;

        public PSAttack(T inputFinish, PlayerManager manager)
        {
            _inputFinish = inputFinish;
            _manager = manager;
        }

        protected virtual void SetWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;
            CurrentAttack = CurrentWeapon.BaseAttack;
        }

        public override void Enter()
        {
            SetWeapon(_manager.weaponController.Weapon);
            CurrentWeapon.Attacking = true;
            CurrentAttack.FinishAnimation += AttackFinished;
            _attack.StartAttack(CurrentAttack, CurrentWeapon);
            _move.ModifySpeed(-CurrentWeapon.SlowPercent);
            
            _manager.Sounds?.Invoke("N_Attack", "Player");
        }

        public override void Execute()
        {
            _attack.ExecuteAttack(CurrentAttack, CurrentWeapon);
            _move.Move(Vector2.zero);
        }

        public override void Exit()
        {
            CurrentWeapon.Attacking = false;
            CurrentWeapon.PutOnCooldown();
            _attack.FinishAttack(CurrentAttack, CurrentWeapon);
            CurrentAttack.FinishAnimation -= AttackFinished;
            _manager.weaponController.CheckDurability();
        }

        protected void AttackFinished()
        {
            StateMachine.Transition(_inputFinish);
            _move.ModifySpeed(CurrentWeapon.SlowPercent);
        }
    }
}