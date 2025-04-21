using System;
using UnityEditor.Tilemaps;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public float range;
    public float angle;
    public LayerMask obstacleMask;

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
    public bool CheckAngle(Transform target)
    {
        return CheckAngle(target, transform.right); //Usamos RIGHT xq en 2D no hay forward.
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
        
        Vector2 forward = transform.right;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, -angle / 2f) * forward * range);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, angle / 2f) * forward * range);
    }
}
