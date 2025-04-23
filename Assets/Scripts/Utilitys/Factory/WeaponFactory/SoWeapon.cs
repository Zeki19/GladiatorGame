using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

namespace Utilitys.Factory.WeaponFactory
{
   [CreateAssetMenu(fileName = "NewWeapon",menuName = "Weapons/SimpleWeapon")]
   public class SoWeapon : ScriptableObject
   {
      public GameObject weaponPrefab;
      public string weaponName;
      public float baseDamage;
      public float attackSpeed;
      public float range;
      public int durability;
      public int durabilityStandardLoss;
      public int durabilityChargeLoss;
      public float changeThreshold;
      public float chargePerAttack;
      public float knockbackForce;
      public float slowPercent;
      [FormerlySerializedAs("BasicAttack")] public SOAttack basicAttack;
   }
}
