using Entities;
using UnityEngine;

public class EnemyModel : EntityModel
{
    protected float _speedModifier = 1;
    public override void Dash(float dashForce)
    {
        throw new System.NotImplementedException();
    }

    public override void ModifySpeed(float speed)
    {
        throw new System.NotImplementedException();
    }

    public override void Move(Vector2 dir)
    {
        throw new System.NotImplementedException();
    }
}
