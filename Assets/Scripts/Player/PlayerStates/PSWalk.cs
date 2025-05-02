using UnityEngine;

public class PSWalk<T> : PSBase<T>
{
    public override void Execute()
    {
        _move.Move(Vector2.zero);
        _look.LookDir(Vector2.zero);
    }
}
