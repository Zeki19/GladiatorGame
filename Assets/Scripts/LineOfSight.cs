using System;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public float range;
    public float angle;
    public LayerMask obstacleMask;
    private Vector2 _forward;

    private void Awake()
    {
        _forward = transform.up;
    }

    public bool CheckRange(Transform target)
    {
        return CheckRange(target,range);
    }
    public bool CheckRange(Transform target, float range)
    {
        Vector2 dir = target.position - transform.position;
        float distance = dir.magnitude;
        return distance <= range;
    }

    private bool CheckAngle(Transform target)
    {
        return CheckAngle(target, _forward);
    }
    private bool CheckAngle(Transform target, Vector2 front)
    {
        Vector2 dir = target.position - transform.position;
        float angleToTarget = Vector2.Angle(front, dir.normalized);
        return angleToTarget <= angle / 2;
    }
    private bool CheckView(Transform target)
    {
        Vector2 dir = target.position - transform.position;
        return !Physics.Raycast(transform.position, dir.normalized, dir.magnitude, obstacleMask);
    }

    public bool LOS(Transform target)
    {
        return CheckRange(target) && CheckAngle(target) && CheckView(target);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,range);
        
        Gizmos.color = Color.yellow;

        var forward = transform.up;
        
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, -angle / 2f) * forward * range);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, angle / 2f) * forward * range);
    }
}
