using System.Collections.Generic;
using UnityEngine;

public class PatrolToPoint : ISteering
{
    private List<Vector2> _waypoints;
    private int _currentIndex = 0;
    private Transform _self;
    private Vector2 dir;
    private Vector2 currentPos;

    public PatrolToPoint(List<Vector2> waypoints, Transform self)
    {
        _waypoints = waypoints;
        _self = self;

        // Optionally: Start at the closest waypoint
        _currentIndex = GetClosestWaypointIndex();
    }
    
    public Vector2 GetDir()
    { 
        ToPoint();
        return dir.normalized;
    }
    
    private void ToPoint()
    {
        currentPos = _self.position;
        Vector2 target = _waypoints[_currentIndex];

        if (Vector2.Distance(currentPos, target) < 0.2f)
        {
            _currentIndex = (_currentIndex + 1) % _waypoints.Count;
        }

        dir = _waypoints[_currentIndex] - currentPos;
    }
    
    private int GetClosestWaypointIndex()
    {
        float minDist = float.MaxValue;
        int closestIndex = 0;

        for (int i = 0; i < _waypoints.Count; i++)
        {
            float dist = Vector2.Distance(_self.position, _waypoints[i]);
            if (dist < minDist)
            {
                minDist = dist;
                closestIndex = i;
            }
        }

        return closestIndex;
    }
}
