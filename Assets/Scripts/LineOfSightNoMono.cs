using System;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class LineOfSightNoMono
{
    private float _range;
    private float _angle;
    private LayerMask _obstacleMask;

    public LineOfSightNoMono(float range, float angle, LayerMask obstacleMask)
    {
        _angle = angle;
        _range = range;
        _obstacleMask = obstacleMask;
    }
    public bool CheckRange(Transform looker, Transform target)
    {
        return CheckRange(looker,target,_range);
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
        return angleToTarget <= _angle / 2;
    }
    private bool CheckView(Transform looker, Transform target)
    {
        Vector2 dir = target.position - looker.position;
        return !Physics2D.Raycast(looker.position, dir.normalized, dir.magnitude, _obstacleMask);
    }

    public bool LOS(Transform looker, Transform target)
    {
        return CheckRange(looker,target) && CheckAngle(looker,target) && CheckView(looker,target);
    }
}
