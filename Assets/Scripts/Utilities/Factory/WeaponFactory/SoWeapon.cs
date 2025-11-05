using Attack;
using UnityEngine;

namespace Utilities.Factory.WeaponFactory
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/SimpleWeapon")]
    public class SoWeapon : ScriptableObject
    {
        public GameObject weaponPrefab;
        public string weaponName;
        public float cooldown;
        public int durability;
        public int durabilityStandardLoss;
        public int durabilityChargeLoss;
        public bool standardConsumeDurabilityOnMiss;
        public bool chargeConsumeDurabilityOnMiss;
        public float changeThreshold;
        public float chargePerAttack;
        public float knockbackForce;
        public float slowPercent;
        public BaseAttack basic;
        public BaseAttack charge;
    }
}