using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LineOfSight2D
{
    public bool CheckRange(Transform self, Transform target, float range)
    {
        Vector2 dir = target.position - self.position;
        float distance = dir.magnitude;
        return distance <= range;
    }
    public bool CheckAngle(Transform self, Transform target, float angle)
    {
        Vector2 dir = (target.position - self.position).normalized;
        float angleToTarget = Vector2.Angle(self.right, dir); 
        return angleToTarget <= angle / 2f;
    }
    public bool CheckView(Transform self, Transform target, LayerMask obsMask)
    {
        Vector2 origin = self.position;
        Vector2 direction = (target.position - self.position).normalized;
        float distance = Vector2.Distance(self.position, target.position);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, obsMask);
        return hit.collider == null;
    }
}