using Weapons;

namespace Player.PlayerStates
{
    public class PSChargeAttack<T> : PSAttack<T>
    {
        public PSChargeAttack(T inputFinish, PlayerManager manager) : base(inputFinish, manager)
        {
            
        }
        protected override void SetWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;
            CurrentAttack = CurrentWeapon.ChargeAttack;
        }

        public override void Enter()
        {
            base.Enter();
            CurrentWeapon.ResetChangeMeter();
        }
    }
}