using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_FollowPoints<T> : States_Base<T>
{
    protected List<Vector3Int> Waypoints;
    private int _index;
    protected Transform Entity;
    protected float DistanceToPoint;
    private bool _isFinishPath;
    protected GridManager GridManager;
    
    protected State_FollowPoints(Transform entity, float distanceToPoint = 0.2f)
    {
        Entity = entity;
        DistanceToPoint = distanceToPoint;
        _isFinishPath = true;
    }
    protected State_FollowPoints(Transform entity, List<Vector3Int> waypoints, float distanceToPoint = 0.2f)
    {
        Entity = entity;
        DistanceToPoint = distanceToPoint;
        Waypoints = waypoints;
        _isFinishPath = true;
    }

    public override void Enter()
    {
        base.Enter();
        GridManager = ServiceLocator.Instance.GetService<GridManager>();
    }
    public override void Execute()
    {
        base.Execute();
        Run();
    }
    protected void SetWaypoints(List<Vector3Int> newPoints)
    {
        if (newPoints.Count == 0) return;
        Waypoints = newPoints;
        _index = 0;
        _isFinishPath = false;
    }
    void Run()
    {
        if (_isFinishPath) return;
        Vector3 point = Waypoints[_index];
        point.z = Entity.position.z;
        Vector3 dir = point - Entity.position;
        if (dir.magnitude < DistanceToPoint)
        {
            Entity.position = Waypoints[_index];
            if (_index + 1 < Waypoints.Count)
                _index++;
            else
            {
                _isFinishPath = true;
                return;
            }
        }
        OnMove(dir.normalized);
    }
    protected virtual void OnMove(Vector3 dir)
    {
        _move.Move(dir);
        _look.LookDir(dir);
    }
    protected bool InView(Vector3Int grandparent, Vector3Int child)
    {
        Vector3Int start = Vector3Int.RoundToInt(grandparent);
        Vector3Int end = Vector3Int.RoundToInt(child);

        int dx = Mathf.Abs(end.x - start.x);
        int dy = Mathf.Abs(end.y - start.y);

        int sx = (start.x < end.x) ? 1 : -1;
        int sy = (start.y < end.y) ? 1 : -1;

        int err = dx - dy;

        int x = start.x;
        int y = start.y;

        while (true)
        {
            Vector3Int current = new Vector3Int(x, y, 0);
            if (!GridManager.IsRightPos(current))
                return false;

            if (x == end.x && y == end.y)
                break;

            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y += sy;
            }
        }

        return true;
    }
    protected List<Vector3Int> GetConnections(Vector3Int curr)
    {
        var neighbours = new List<Vector3Int>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                var child = new Vector3Int(x, y, 0) + curr;
                if (GridManager.IsRightPos(child))
                {
                    neighbours.Add(child);
                }
            }
        }
        return neighbours;
    }
    protected float GetCost(Vector3Int current, Vector3Int child)
    {
        var baseCost = 5f;
        
        if (GridManager.NextToWall.ContainsKey(child))
        {
            baseCost += 3f; 
        }
        
        if (GridManager.PickUpItem.TryGetValue(child, out float pickupValue))
        {
            baseCost -= pickupValue;
        }

        return baseCost;
    }
    public bool IsFinishPath => _isFinishPath;
}
