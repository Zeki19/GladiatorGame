using Attack;
using Enemies;
using UnityEngine;
using Weapons;
namespace Player.PlayerStates
{
    public class PSAttack<T> : PSBase<T>
    {
        private readonly T _inputFinish;
        private readonly PlayerManager _manager;
        protected BaseAttack CurrentAttack;
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
            CurrentAttack.AttackFinish += AttackFinished;
            //_attack.ExecuteAttack(CurrentAttack, CurrentWeapon);
            _move.ModifySpeed(-CurrentWeapon.SlowPercent);
            CurrentAttack.ExecuteAttack();
            
            
            _sound.PlaySound("N_Attack", "Player");
        }

        public override void Execute()
        {
            //_attack.ExecuteAttack(CurrentAttack, CurrentWeapon);
            _move.Move(Vector2.zero);
        }

        public override void Exit()
        {
            CurrentWeapon.Attacking = false;
            CurrentWeapon.PutOnCooldown();
            //_attack.FinishAttack(CurrentAttack, CurrentWeapon);
            CurrentAttack.AttackFinish -= AttackFinished;
            _manager.weaponController.CheckDurability();
        }

        protected void AttackFinished()
        {
            StateMachine.Transition(_inputFinish);
            _move.ModifySpeed(CurrentWeapon.SlowPercent);
        }
    }
}