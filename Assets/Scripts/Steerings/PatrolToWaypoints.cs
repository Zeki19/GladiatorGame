using System.Collections.Generic;
using UnityEngine;

public class PatrolToWaypoints : ISteering
{
    private List<Vector2> _waypoints;
    private int _currentIndex = 0;
    private Transform _self;
    private Vector2 dir;
    private Vector2 currentPos;
    private bool _goingForward = true;

    public PatrolToWaypoints(List<Vector2> waypoints, Transform self)
    {
        _waypoints = waypoints;
        _self = self;

        _currentIndex = 0;
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
            if (_goingForward)
            {
                _currentIndex++;
                if (_currentIndex >= _waypoints.Count)
                {
                    _currentIndex = _waypoints.Count - 2;
                    _goingForward = false;
                }
            }
            else
            {
                _currentIndex--;
                if (_currentIndex < 0)
                {
                    _currentIndex = 1;
                    _goingForward = true;
                }
            }
        }
        
        dir = _waypoints[_currentIndex] - currentPos;
    }
}
