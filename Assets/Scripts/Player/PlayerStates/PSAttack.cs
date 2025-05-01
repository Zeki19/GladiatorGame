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
        public PSAttack(T inputFinish, float seconds = 1)
        {
            _inputFinish = inputFinish;
            _seconds = seconds;
        }

        public void SetWeapon(Attack attack,Weapon weapon)
        {
            _currentAttack = attack;
            _weapon = weapon;
        }
        public override void Enter()
        {
            base.Enter();
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
    }
}
