using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove
{
    void ModifySpeed(float speed);
    void Move(float moveSpeed);
    void Dash( float dashForce);
    Vector2 Position { get; }
}
