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
            CurrentAttack.FinishAnimation += AttackFinished;
            _attack.StartAttack(CurrentAttack, CurrentWeapon);
            _move.ModifySpeed(-CurrentWeapon.SlowPercent);
            
            _manager.Sounds?.Invoke("C_Attack", "Player");
            CurrentWeapon.ResetChangeMeter();
        }
    }
}