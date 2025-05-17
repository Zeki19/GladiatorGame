using UnityEngine;
using Weapons.Attacks;

namespace Weapons.AttackSo
{
    [CreateAssetMenu(fileName = "NewWeapon",menuName = "AttackSo/Poke")]
    public class PokeAttackSo : AttackSo
    {
        [SerializeField] private int pierce;
        [SerializeField] private AnimationCurve curve;
        public override Attack Clone()
        {
            return new Poke(pierce,curve);
        }
    }
}