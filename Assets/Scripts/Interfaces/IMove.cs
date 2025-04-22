using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove
{
    void ModifySpeed(float speed);
    void Move(Vector2 dir);
    void Dash( float dashForce);
    Vector2 Position { get; }
}
