using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(fileName = "NewWeapon",menuName = "SOAttack/Swing")]
    public class SoSwing : SOAttack
    {
        [SerializeField] private float swingStartingPoint;
        [SerializeField] private float swingAngle;
        public override Attack Clone()
        {
            return new BasicSwing(swingStartingPoint,swingAngle);
        }
    }
}