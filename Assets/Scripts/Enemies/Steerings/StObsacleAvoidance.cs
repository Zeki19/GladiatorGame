using UnityEngine;

public class StObstacleAvoidance
{
    [Min(1)]
    private int _maxObs = 1;
    [Min(0)]
    private float _radius;
    [Min(1)]
    private float _angle;

    private float _personalArea;
    public LayerMask _obsMask;
    Collider2D[] _colls;
    public StObstacleAvoidance(int maxObs, float radius, float angle, float personalArea, LayerMask obsMask)
    {
        _radius = radius;
        _angle = angle;
        _personalArea = personalArea;
        _obsMask = obsMask;
        _maxObs = maxObs;
        _colls = new Collider2D[_maxObs];
    }
    public Vector2 GetDir(Transform self, Vector2 currDir)
    {
        _colls = Physics2D.OverlapCircleAll(self.position, _radius, _obsMask);
        int count = _colls.Length;
        //int count = Physics2D.OverlapCircle(Self, radius, contactFilter, _colliders); <- INTENTO DE NACHO DE QUE ANDE NONALLOC.
        Collider2D nearColl = null;
        float nearCollDistance = 0;
        Vector3 nearClosestPoint = Vector2.zero;
        for (int i = 0; i < count; i++)
        {
            Collider2D currColl = _colls[i];
            Vector2 closestPoint = currColl.ClosestPoint(self.position);
            Vector2 dir = closestPoint - new Vector2(self.position.x, self.position.y);
            float distance = dir.magnitude;

            var currAngle = Vector2.Angle(dir, currDir);
            if (currAngle > _angle ) continue;
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
        //Vector2 relativePos = self.InverseTransformPoint(nearClosestPoint);
        Vector3 dirToColl = (nearClosestPoint - self.position).normalized;
        Vector2 avoidDir = Vector2.Perpendicular(dirToColl).normalized;
        return Vector2.Lerp(currDir, avoidDir, (_radius - Mathf.Clamp(nearCollDistance - _personalArea, 0, _radius)) / _radius);
        /*
        Vector3 avoidanceDir = Vector3.Cross(Vector3.up, dirToColl);
        Vector2 avoidanceDir2D = new Vector2(avoidanceDir.x, avoidanceDir.z);

        if (relativePos.x > 0)
        {
            avoidanceDir2D = -avoidanceDir2D;
        }

        
        if (avoidanceDir2D == Vector2.zero || Mathf.Abs(avoidanceDir2D.x) != 1)
        {
            avoidanceDir2D = new Vector2(currDir.normalized.x, 0);

        }
        Debug.DrawRay(self.position, avoidanceDir2D * 2, Color.red);

        return Vector2.Lerp(currDir, avoidanceDir2D, (_radius - Mathf.Clamp(nearCollDistance - _personalArea, 0, _radius)) / _radius);
        */
    }
}

