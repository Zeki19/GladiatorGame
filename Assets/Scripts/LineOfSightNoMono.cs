using System;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class LineOfSightNoMono
{
    public float range;
    public float angle;
    public LayerMask obstacleMask;

    public bool CheckRange(Transform looker, Transform target)
    {
        return CheckRange(looker,target,range);
    }
    public bool CheckRange(Transform looker, Transform target, float range)
    {
        Vector2 dir = target.position - looker.position;
        float distance = dir.magnitude;
        return distance <= range;
    }

    private bool CheckAngle(Transform looker, Transform target)
    {
        return CheckAngle(looker, target, looker.up);
    }
    private bool CheckAngle(Transform looker, Transform target, Vector2 front)
    {
        Vector2 dir = target.position - looker.position;
        float angleToTarget = Vector2.Angle(front, dir.normalized);
        return angleToTarget <= angle / 2;
    }
    private bool CheckView(Transform looker, Transform target)
    {
        Vector2 dir = target.position - looker.position;
        return !Physics.Raycast(looker.position, dir.normalized, dir.magnitude, obstacleMask);
    }

    public bool LOS(Transform looker, Transform target)
    {
        return CheckRange(looker,target) && CheckAngle(looker,target) && CheckView(looker,target);
    }
}
