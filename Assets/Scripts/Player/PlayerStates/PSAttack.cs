using UnityEngine;
using Weapons;

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
            _currentAttack = weapon._baseSoAttack;
            _weapon = weapon;
        }
        public override void Enter()
        {
            base.Enter();
            SetWeapon(_manager.weapon);
            _currentAttack.FinishAnimation += AttackFinished;
            _attack.StartAttack(_currentAttack,_weapon);
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
            _attack.FinishAttack(_currentAttack,_weapon);
            _currentAttack.FinishAnimation -= AttackFinished;
        }

        private void AttackFinished()
        {
            StateMachine.Transition(_inputFinish);
        }
    }
}
