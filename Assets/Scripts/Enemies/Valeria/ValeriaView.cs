using System;
using Enemies;
using Entities.Interfaces;
using Unity.Mathematics;
using UnityEngine;

public class ValeriaView : EnemyView, ILook
{
    public override void LookDir(Vector2 dir)
    {
        if (dir == Vector2.zero) return;

        float angle = Vector2.SignedAngle(Vector2.up, dir);

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public override void LookDirInsta(Vector2 dir)
    {
        float angle = Vector2.SignedAngle(Vector2.up, dir);
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        transform.rotation = targetRotation;
    }
}
