using UnityEngine;

namespace Entities.Interfaces
{
    public interface IMove
    {
        void ModifySpeed(float speed);
        void Move(Vector2 dir);
        void Dash(float dashForce);
        void Dash(Vector2 dir,float dashForce);
        void Dash(Vector2 dir,float dashForce, float backStepDistance);
        void SetLinearVelocity(Vector2 velocity);
        void StopMovement();
        Vector2 Position { get; }
    }
}
