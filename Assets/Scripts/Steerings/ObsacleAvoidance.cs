using UnityEngine;

public class ObstacleAvoidance
{
    [Min(1)]
    private int _maxObs = 2;
    [Min(0)]
    private float _radius;
    [Min(1)]
    private float _angle;

    private float _personalArea;
    public LayerMask _obsMask;
    Collider[] _colls;
    public ObstacleAvoidance(int maxObs, float radius, float angle, float personalArea, LayerMask obsMask)
    {
        _colls = new Collider[maxObs];
        _radius = radius;
        _angle = angle;
        _personalArea = personalArea;
        _obsMask = obsMask;
    }
    public Vector3 GetDir(Transform self, Vector3 currDir)
    {
        int count = Physics.OverlapSphereNonAlloc(self.position, _radius, _colls, _obsMask);

        Collider nearColl = null;
        float nearCollDistance = 0;
        Vector3 nearClosestPoint = Vector3.zero;
        for (int i = 0; i < count; i++)
        {
            Collider currColl = _colls[i];
            Vector3 closestPoint = currColl.ClosestPoint(self.position);
            Vector3 dir = closestPoint - self.position;
            float distance = dir.magnitude;

            var currAngle = Vector3.Angle(dir, currDir);
            if (currAngle > _angle / 2) continue;
            if (nearColl == null || distance < nearCollDistance)
            {
                nearColl = currColl;
                nearCollDistance = distance;
                nearClosestPoint = closestPoint;
            }
        }

        if (nearColl == null)
        {
            return currDir;
        }

        Vector3 relativePos = self.InverseTransformPoint(nearClosestPoint);
        Vector3 dirToColl = (nearClosestPoint - self.position).normalized;
        Vector3 avoidanceDir = Vector3.Cross(self.up, dirToColl);
        if (relativePos.x > 0)
        {
            avoidanceDir = -avoidanceDir;
        }
        Debug.DrawRay(self.position, avoidanceDir * 2, Color.red);

        return Vector3.Lerp(currDir, avoidanceDir, (_radius - Mathf.Clamp(nearCollDistance - _personalArea, 0, _radius)) / _radius);
    }
}

