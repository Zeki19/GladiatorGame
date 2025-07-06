using Entities;
using UnityEngine.Serialization;
using Weapons;

namespace Player
{
    public class PlayerManager : EntityManager
    {
        public PlayerStats stats;
        public PlayerWeaponController weaponController;

        public DashIcon dashIconUI; 

        private void Awake()
        {
            ServiceLocator.Instance.RegisterService(this);
            HealthSystem = new HealthSystem.HealthSystem(stats.MaxHealth);
        }
    }
}