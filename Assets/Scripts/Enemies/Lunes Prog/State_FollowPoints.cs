using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_FollowPoints<T> : States_Base<T>
{
    protected List<Vector3> Waypoints;
    private int _index;
    protected Transform Entity;
    protected float DistanceToPoint; //How close to be considered "arrived".
    protected bool _isFinishPath;
    public State_FollowPoints(Transform entity, float distanceToPoint = 0.2f)
    {
        Entity = entity;
        DistanceToPoint = distanceToPoint;
        _isFinishPath = true;
    }
    public State_FollowPoints(Transform entity, List<Vector3> waypoints, float distanceToPoint = 0.2f)
    {
        Entity = entity;
        DistanceToPoint = distanceToPoint;
        Waypoints = waypoints;
        _isFinishPath = true;
    }

    public override void Execute()
    {
        base.Execute();
        Run();
    }

    public void SetWaypoints(List<Vector3> newPoints)
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
        point.y = Entity.position.y;
        Vector3 dir = point - Entity.position;
        if (dir.magnitude < DistanceToPoint)
        {
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

    }
    public bool IsFinishPath => _isFinishPath;
}
