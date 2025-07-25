﻿using Entities.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class StatueState_SearchingWall<T> : StatesBase<T>
{
    WallFinder _wallFinder;
    T _inputFinish;
    StatueController _controller;
    Vector2 dir; 
    Vector2 wall;
    public StatueState_SearchingWall(WallFinder DetectClosestWall, T InputFinish, StatueController StateManager)
    {
        _wallFinder = DetectClosestWall;
        _inputFinish = InputFinish;
        _controller = StateManager;
    }

    public override void Enter()
    {
        base.Enter();

        wall = _wallFinder.ClosestPoint(Vector2.zero);
        _controller._wallPosition = wall;
        Debug.Log(wall);
    }

    public override void Execute()
    {
        base.Execute();
    }

}
