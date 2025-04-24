using UnityEngine;
using UnityEngine.InputSystem.XR;

public class StatueState_SearchingWall<T> : States_Base<T>
{
    ObstacleAvoidance _obstacleAvoidance;
    T _inputFinish;
    StatueController _controller;
    Vector2 dir; 
    Vector2 wall;
    public StatueState_SearchingWall(ObstacleAvoidance DetectClosestWall, T InputFinish, StatueController StateManager)
    {
        _obstacleAvoidance = DetectClosestWall;
        _inputFinish = InputFinish;
        _controller = StateManager;
    }

    public override void Enter()
    {
        base.Enter();

        wall = _obstacleAvoidance.ClosestPoint(Vector2.zero);
        _controller._wallPosition = wall;
        Debug.Log(wall);
    }

    public override void Execute()
    {
        base.Execute();
    }

}
