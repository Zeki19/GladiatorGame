using Weapons;

namespace Player.PlayerStates
{
    public class PSChargeAttack<T> : PSAttack<T>
    {
        private PlayerManager _manager;
        public PSChargeAttack(T inputFinish, PlayerManager manager) : base(inputFinish, manager)
        {
            _manager = manager;
        }
        protected override void SetWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;
            CurrentAttack = CurrentWeapon.ChargeAttack;
        }

        public override void Enter()
        {
            SetWeapon(_manager.weaponController.Weapon);
            CurrentWeapon.Attacking = true;
            CurrentAttack.AttackFinish += AttackFinished;
            CurrentAttack.ExecuteAttack();
            //_attack.StartAttack(CurrentAttack, CurrentWeapon);
            _move.ModifySpeed(-CurrentWeapon.SlowPercent);
            
            _sound.PlaySound("C_Attack", "Player");
            _manager.PlaySound("C_Attack");
            CurrentWeapon.ResetChangeMeter();
        }
    }
}