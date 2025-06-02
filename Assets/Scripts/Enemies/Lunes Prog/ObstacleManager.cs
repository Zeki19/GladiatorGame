using UnityEngine;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour
{
    Dictionary<Vector3, int> _obs = new Dictionary<Vector3, int>();
    static ObstacleManager instance;
    public static ObstacleManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("ObstacleManager").AddComponent<ObstacleManager>();
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
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
    private void OnDrawGizmosSelected()
    {
        if (_obs == null) return;
        Gizmos.color = Color.red;
        foreach (var item in _obs)
        {
            Gizmos.DrawWireSphere(item.Key, 0.25f);
        }
    }
}
