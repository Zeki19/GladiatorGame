using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(fileName = "NewWeapon",menuName = "AttackSo/Swing")]
    public class BasicSwingAttackSo : AttackSo.AttackSo
    {
        [SerializeField] private float swingStartingPoint;
        [SerializeField] private float swingAngle;
        public override Attack Clone()
        {
            return new BasicSwing(swingStartingPoint,swingAngle);
        }
    }
}