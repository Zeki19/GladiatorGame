using Enemies.Gaius;
using Entities;
using UnityEngine;

public class GaiusStateChase<T> : State_Steering<T>
{
    private SpriteRenderer _spriteRenderer;
    private EntityManager _gaiusManager;
    private float SpeedMod;
    private float StackingSpeed;
    private float SpeedModeInterval = 1;
    private float Timer;
    public GaiusStateChase(ISteering steering, StObstacleAvoidance stObstacleAvoidance, Transform self, SpriteRenderer spriteRenderer,EntityManager manager) : base(steering, stObstacleAvoidance, self)
    {
        _spriteRenderer = spriteRenderer;
        _gaiusManager = manager;
        var gauisControler = _gaiusManager.controller as GaiusController;
        SpeedMod = gauisControler.stats.Stack;
        SpeedModeInterval = gauisControler.stats.Interval;
    }

    public override void Enter()
    {
        base.Enter();
        _move.Move(Vector2.zero);
        _gaiusManager.view.PlayStateAnimation(StateEnum.Chase);
        Timer = SpeedModeInterval;
    }
    public override void Execute()
    {
        base.Execute();

        Vector2 dir = _steering.GetDir();
        Timer -= Time.deltaTime;
        if(Timer<0)
        {
            _move.ModifySpeed(SpeedMod);
            StackingSpeed += SpeedMod;
            Timer = SpeedModeInterval;
        }
        _move.Move(dir);
        _look.LookDir(dir);
    }
    public override void Exit()
    {
        base.Exit();
        _move.ModifySpeed(-StackingSpeed);
        StackingSpeed = 0;
    }
}
