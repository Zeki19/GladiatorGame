using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PSWalk<T> : PSBase<T>
{
    public override void Execute()
    {
        _move.Move(Vector2.zero);
    }
}
