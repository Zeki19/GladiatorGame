using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove
{
    void Move(Vector2 dir);
    void Dash();

    Vector2 Position { get; }
}
