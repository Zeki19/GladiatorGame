using UnityEngine;
using Weapons;

namespace Player.PlayerStates
{
    public class PSAttack<T> : PSBase<T>
    {
        float _timer;
        float _seconds = 1;
        T _inputFinish;
        private Attack _currentAttack;
        private Weapon _weapon;
        private PlayerManager manager;
        public PSAttack(T inputFinish,PlayerManager manager ,float seconds = 1)
        {
            _inputFinish = inputFinish;
            _seconds = seconds;
            this.manager = manager;
        }

        public void SetWeapon(Weapon weapon)
        {
            _currentAttack = weapon._baseSoAttack;
            _weapon = weapon;
        }
        public override void Enter()
        {
            base.Enter();
            SetWeapon(manager.weapon);
            _timer = _seconds;
            _attack.StartAttack(_currentAttack,_weapon);;
        }
        public override void Execute()
        {
            base.Execute();
            _attack.ExecuteAttack(_currentAttack,_weapon);
            _move.Move(Vector2.zero);
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                StateMachine.Transition(_inputFinish);
            }

        }

        public override void Exit()
        {
            base.Exit();
            _attack.FinishAttack(_currentAttack,_weapon);
        }
    }
}
