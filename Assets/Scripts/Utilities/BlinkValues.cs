using UnityEngine;

namespace Utilities
{
    [CreateAssetMenu(fileName = "BlinkData", menuName = "ScriptableObjects/SpriteEffectData")]
    public class BlinkValues: ScriptableObject
    {
        public int amount;
        public float frequency;
        public Color blinkActive;
    }
}