using UnityEngine;
public class StatueState_Chase<T> : States_Base<T>
{
    private ISteering _steering;
    ObstacleAvoidance _obs;
    StatueController _controller;
    public StatueState_Chase(StatueController controller,ISteering steering, int maxObs, float radius, float angle, float personalArea, LayerMask avoidMask)
    {
        _steering = steering;
        _obs = new ObstacleAvoidance(maxObs, radius, angle, personalArea, avoidMask);
        _controller = controller;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Statue enters Chase state");
    }

    public override void Execute()
    {
        base.Execute();
        var dir = _steering.GetDir();
        dir = _obs.GetDir(_controller.transform ,dir);

        _move.Move(dir.normalized);
        _look.LookDir(dir.normalized);
    }
}
