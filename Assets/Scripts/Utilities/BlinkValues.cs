using UnityEngine;

namespace Utilities
{
    [CreateAssetMenu(fileName = "BlinkData", menuName = "Scriptable Objects/SpriteEffectData")]
    public class BlinkValues: ScriptableObject
    {
        public int amount;
        public float frequency;
        public Color blinkActive;
    }
}