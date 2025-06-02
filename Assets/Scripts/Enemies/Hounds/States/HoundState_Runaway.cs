using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class HoundState_Runaway<T> : State_FollowPoints<T>
{
    private Transform _target;
    public HoundState_Runaway(Transform entity, float distanceToPoint, Transform target) : base(entity, distanceToPoint)
    {
        Entity = entity;
        DistanceToPoint = distanceToPoint;
        _target = target;
    }
    public HoundState_Runaway(Transform entity, float distanceToPoint, Transform target, List<Vector3> waypoints) : base(entity, waypoints, distanceToPoint)
    {
        Entity = entity;
        DistanceToPoint = distanceToPoint;
        _target = target;
        Waypoints = waypoints;
        _isFinishPath = true;
    }
    public override void Enter()
    {
        base.Enter();
        SetPath();
        _move.ModifySpeed(3f);
        _look.PlayStateAnimation(StateEnum.Runaway);
    }
    private void SetPath()
    {
        Vector3 init = Vector3Int.RoundToInt(Entity.transform.position);
        List<Vector3> path = ASTAR.Run<Vector3>(init, IsSatisfied, GetConnections, GetCost, Heuristic);
        path = ASTAR.CleanPath(path, InView);

        SetWaypoints(path);
    }
    protected override void OnMove(Vector3 dir)
    {
        base.OnMove(dir);
        _move.Move(dir);
        //_move.LookDir(dir);
    }
    
    #region Utils
        private bool InView(Vector3 grandparent, Vector3 child)
        {
            var diff = child - grandparent;
            return !Physics.Raycast(grandparent, diff.normalized, diff.magnitude, LayerMask.GetMask("Wall"));
        }
        private float Heuristic(Vector3 current)
        {
            float distanceMultiplier = 1;

            float h = 0;
            h += Vector3.Distance(current, _target.transform.position) * distanceMultiplier;
            return h;
        }
        private float GetCost(Vector3 parent, Vector3 child)
        {
            float distanceMultiplier = 1;

            float cost = 0;
            cost += Vector3.Distance(parent, child) * distanceMultiplier;
            return cost;
        }
        bool IsSatisfied(Vector3 curr)
        {
            return !(Vector3.Distance(curr, _target.transform.position) > 1.25f) && InView(curr, _target.transform.position);
        }
        private List<Vector3> GetConnections(Vector3 curr)
        {
            var neighbours = new List<Vector3>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    var child = new Vector3(x, y, 0) + curr;
                    if (ObstacleManager.Instance.IsRightPos(child))
                    {
                        neighbours.Add(child);
                    }
                }
            }
            return neighbours;
        }
    #endregion

}
