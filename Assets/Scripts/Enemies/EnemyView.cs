using Entities;
using UnityEngine;

public class EnemyView : EntityView
{
    [SerializeField] protected float rotationSpeed;
    public override void LookDir(Vector2 dir){}
    public override void PlayStateAnimation(StateEnum state){}
}
