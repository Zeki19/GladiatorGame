using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSIdle<T> : PSBase<T>
{
    public override void Execute()
    {
        _look.LookDir(Vector2.zero);
    }
}
