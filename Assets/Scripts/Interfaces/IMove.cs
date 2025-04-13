using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove
{
    void Move(Vector2 dir, float moveSpeed);
    void Dash( float dashForce);

    Vector2 Position { get; }
}
