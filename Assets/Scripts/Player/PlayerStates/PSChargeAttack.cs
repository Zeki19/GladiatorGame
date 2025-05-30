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
            CurrentAttack = weapon.ChargeAttack;
            Weapon = weapon;
        }

        public override void Enter()
        {
            base.Enter();
            Weapon.ResetChangeMeter();
        }
    }
}