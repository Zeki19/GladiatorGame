using UnityEngine;

namespace Entities.Interfaces
{
    public interface IKnockbackable
    {
        protected float KnockbackWeight { get; }
        protected bool CanBeKnockedBack { get; }
        /// <summary>
        /// Knockback target with a given force
        /// </summary>
        /// <param name="force">direction * intensity</param>
        public void ApplyKnockback(Vector2 force);
        /// <summary>
        /// This is a version of knockback if the one applying the force don't know the receiver direction
        /// </summary>
        /// <param name="sourcePosition">Direction</param>
        /// <param name="intensity">intensity</param>
        public void ApplyKnockbackFromSource(Vector2 sourcePosition, float intensity);

    }
}
