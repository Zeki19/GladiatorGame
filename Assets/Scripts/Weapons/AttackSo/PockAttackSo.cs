using UnityEngine;
using Weapons.Attacks;

namespace Weapons.AttackSo
{
    [CreateAssetMenu(fileName = "NewWeapon",menuName = "AttackSo/Pock")]
    public class PockAttackSo : AttackSo
    {
        [SerializeField] private float maxDistance;
        [SerializeField] private int pierce;
        public override Attack Clone()
        {
            return new Pock(maxDistance, pierce);
        }
    }
}