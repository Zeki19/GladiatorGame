using UnityEngine;

public class FirstBossModel : EnemyModel
{
    public override void ModifySpeed(float speed)
    {
        _speedModifier += speed;
    }

    public override void Move(Vector2 dir)
    {
        dir.Normalize(); //Just in case someone fucks up.
        manager.Rb.linearVelocity = dir * (moveSpeed * _speedModifier);
    }
}
