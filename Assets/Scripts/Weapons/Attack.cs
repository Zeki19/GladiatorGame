namespace Weapons
{
    public abstract class Attack
    {
        public abstract void StartAttack(Weapon weapon);
        public abstract void ExecuteAttack(Weapon weapon);
        public abstract void FinishAttack(Weapon weapon);
    }
}
