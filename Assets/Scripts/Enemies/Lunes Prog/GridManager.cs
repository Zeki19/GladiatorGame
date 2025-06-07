using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int WeightOfPointsNextToWalls;
    
    private readonly Dictionary<Vector3, int> _walls = new Dictionary<Vector3, int>();
    public readonly Dictionary<Vector3, int> NextToWall = new Dictionary<Vector3, int>();
    public readonly Dictionary<Vector3Int, float> PickUp = new Dictionary<Vector3Int, float>();
    
    private void Awake()
    {
        ServiceLocator.Instance.RegisterService(this);
    }
    private void Start()
    {
        foreach (Transform child in transform)
        {
            Collider2D coll = child.GetComponent<Collider2D>();
            if (coll != null)
            {
                AddColl(coll);
            }
        }
    }
    private void AddColl(Collider2D coll)
    {
        var points = GetPointsOnCollider(coll);
    
        //RED points
        for (int i = 0; i < points.Count; i++)
        {
            if (_walls.ContainsKey(points[i]))
            {
                _walls[points[i]]++;
            }
            else
            {
                _walls[points[i]] = 1;
            }
        }

        //Clean YELLOW
        foreach (var p in points)
        {
            if (NextToWall.ContainsKey(p))
            {
                NextToWall.Remove(p);
            }
        }

        //YELLOW points
        var steps = GetPointsAroundColl(points);
        for (int i = 0; i < steps.Count; i++)
        {
            if (!NextToWall.ContainsKey(steps[i]))
            {
                NextToWall.Add(steps[i], WeightOfPointsNextToWalls);
            }
            else
            {
                NextToWall[steps[i]]++;
            }
        }
    }
    public bool IsRightPos(Vector3 curr)
    {
        curr = Vector3Int.RoundToInt(curr);
        return !_walls.ContainsKey(curr);
    }
    private List<Vector3> GetPointsOnCollider(Collider2D coll)
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
    private List<Vector3> GetPointsAroundColl(List<Vector3> pts)
    {
        var result = new HashSet<Vector3>();

        foreach (var curr in pts.Select(Vector3Int.RoundToInt))
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;

                    Vector3Int neighbor = new Vector3Int(curr.x + x, curr.y + y, 0);

                    if (!_walls.ContainsKey(neighbor))
                    {
                        result.Add(neighbor);
                    }
                }
            }
        }
        return new List<Vector3>(result); 
    }
    public void AddPickUp(Vector3 centerPos, int weight, int range)
    {
        Vector3Int center = Vector3Int.RoundToInt(centerPos);

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                Vector3Int p = new Vector3Int(center.x + x, center.y + y, 0);

                if (_walls.ContainsKey(p)) continue;

                int dist = Mathf.Abs(x) + Mathf.Abs(y);
                if (dist > range) continue;

                float falloff = 1f - (dist / (float)range);
                float pointValue = Mathf.Max(0f, weight * falloff);

                if (pointValue <= 0f) continue;

                if (!PickUp.TryAdd(p, Mathf.RoundToInt(pointValue)))
                {
                    PickUp[p] += Mathf.RoundToInt(pointValue);
                }
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (_walls == null) return;
        Gizmos.color = Color.red;
        foreach (var item in _walls)
        {
            Gizmos.DrawWireSphere(item.Key, 0.25f);
        }

        if (NextToWall == null) return;
        foreach (var item in NextToWall)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(item.Key, 0.25f);
        }

        if (PickUp != null)
        {
            foreach (var item in PickUp)
            {
                var t = Mathf.InverseLerp(0, 20, item.Value);
                Gizmos.color = Color.Lerp(Color.red, Color.green, t);
                Gizmos.DrawWireSphere(item.Key, 0.25f);
            }
        }
    }
}
