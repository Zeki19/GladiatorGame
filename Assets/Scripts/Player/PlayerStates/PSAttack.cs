using UnityEngine;
using Weapons;
using Weapons.Attacks;

namespace Player.PlayerStates
{
    public class PSAttack<T> : PSBase<T>
    {
        T _inputFinish;
        private Attack _currentAttack;
        private Weapon _weapon;
        private PlayerManager _manager;
        public PSAttack(T inputFinish,PlayerManager manager)
        {
            _inputFinish = inputFinish;
            _manager = manager;
        }

        public void SetWeapon(Weapon weapon)
        {
            _currentAttack = weapon.BaseSoAttack;
            _weapon = weapon;
        }
        public override void Enter()
        {
            base.Enter();
            SetWeapon(_manager.weapon);
            _weapon.Attacking = true;
            _currentAttack.FinishAnimation += AttackFinished;
            _attack.StartAttack(_currentAttack,_weapon);
            _move.ModifySpeed(-_weapon.SlowPercent);
        }
        public override void Execute()
        {
            base.Execute();
            _attack.ExecuteAttack(_currentAttack,_weapon);
            _move.Move(Vector2.zero);
        }

        public override void Exit()
        {
            base.Exit();
            _weapon.Attacking = false;
            _attack.FinishAttack(_currentAttack,_weapon);
            _currentAttack.FinishAnimation -= AttackFinished;
        }

        private void AttackFinished()
        {
            StateMachine.Transition(_inputFinish);
            _move.ModifySpeed(_weapon.SlowPercent);
        }
    }
}
