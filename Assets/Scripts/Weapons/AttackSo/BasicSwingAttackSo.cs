using UnityEngine;
using Weapons.Attacks;

namespace Weapons.AttackSo
{
    [CreateAssetMenu(fileName = "NewWeapon",menuName = "AttackSo/Swing")]
    public class BasicSwingAttackSo : AttackSo
    {
        [SerializeField] private float swingStartingAngle;
        [SerializeField] private AnimationCurve curve;
        public override Attack Clone()
        {
            return new BasicSwing(swingStartingAngle,curve);
        }
    }
}