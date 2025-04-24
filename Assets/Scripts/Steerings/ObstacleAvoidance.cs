using UnityEngine;

public class ObstacleAvoidance : MonoBehaviour
{
    Collider2D[] _colliders;
    int radius = 10;
    public LayerMask obsMask;
    private ContactFilter2D contactFilter;
    Vector3 Self =>gameObject.transform.position;
    private void Start()
    {
        contactFilter.SetLayerMask(obsMask);
    }
    public Vector2 ClosestPoint(Vector2 currDir)
    {
        _colliders = Physics2D.OverlapCircleAll(Self, radius, obsMask);
        //int count = Physics2D.OverlapCircle(Self, radius, contactFilter, _colliders);
        int count = _colliders.Length;
        Collider2D nearColl = null;
        float nearCollDistance = 0;
        for (int i = 0; i < count; i++)
        {
            Collider2D currColl = _colliders[i];
            Vector2 closestPoint = currColl.ClosestPoint(Self);
            Vector2 dir = closestPoint - new Vector2(Self.x,Self.y);
            float distance = dir.magnitude;
            if (nearColl == null || distance < nearCollDistance)
            {
                nearColl = currColl;
                nearCollDistance = distance;
            }
        }
        if (nearColl == null) return currDir;
        return nearColl.ClosestPoint(Self) - new Vector2(Self.x,Self.y);
    }
}
