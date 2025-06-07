using UnityEngine;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour
{
    Dictionary<Vector3, int> _obs = new Dictionary<Vector3, int>();
    public Dictionary<Vector3, int> _pickup = new Dictionary<Vector3, int>(); //Solo para Getter
    private void Awake()
    {
        ServiceLocator.Instance.RegisterService(this);
    }
    
    public void AddColl(Collider2D coll)
    {
        var points = GetPointsOnCollider(coll);
        for (int i = 0; i < points.Count; i++)
        {
            if (_obs.ContainsKey(points[i]))
            {
                _obs[points[i]]++;
            }
            else
            {
                _obs[points[i]] = 1;
            }
        }

        var steps = GetAroundPoints(points);
        for (int i = 0; i < steps.Count; i++)
        {
            if (!_pickup.ContainsKey(steps[i]))
            {
                _pickup.Add(steps[i],8);
            }
            else
            {
                _pickup[steps[i]]++;
            }
        }
    }
    public void RemoveColl(Collider2D coll)
    {
        var points = GetPointsOnCollider(coll);
        for (int i = 0; i < points.Count; i++)
        {
            if (_obs.ContainsKey(points[i]))
            {
                _obs[points[i]] -= 1;
                if (_obs[points[i]] <= 0)
                {
                    _obs.Remove(points[i]);
                }
            }
        }
    }
    public bool IsRightPos(Vector3 curr)
    {
        curr = Vector3Int.RoundToInt(curr);
        return !_obs.ContainsKey(curr);
    }
    List<Vector3> GetPointsOnCollider(Collider2D coll)
    {
        List<Vector3> points = new List<Vector3>();
        Bounds bounds = coll.bounds;

        int minX = Mathf.FloorToInt(bounds.min.x);
        int maxX = Mathf.CeilToInt(bounds.max.x);

        int minY = Mathf.FloorToInt(bounds.min.y);
        int maxY = Mathf.CeilToInt(bounds.max.y);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            { 
                Vector3 point = new Vector3(x, y, 0);
                if (bounds.Contains(point))
                {
                    points.Add(point);
                }
            }
        }

        return points;
    }

    List<Vector3> GetAroundPoints(List<Vector3> pts)
    {
        var result = new HashSet<Vector3>(); // Use HashSet to avoid duplicates

        foreach (var step in pts)
        {
            Vector3Int curr = Vector3Int.RoundToInt(step);

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue; // Skip center point

                    Vector3Int neighbor = new Vector3Int(curr.x + x, curr.y + y, 0);

                    if (!_obs.ContainsKey(neighbor)) // Only add if not an obstacle
                    {
                        result.Add(neighbor);
                    }
                }
            }
        }
        return new List<Vector3>(result); 
    }

    public int Count()
    {
        return _obs.Count;
    }
    private void OnDrawGizmosSelected()
    {
        
        if (_pickup == null) return;
        Gizmos.color = Color.yellow;
        foreach (var item in _pickup)
        {
            Gizmos.DrawWireSphere(item.Key, 0.25f);
        }
    }
    
}
