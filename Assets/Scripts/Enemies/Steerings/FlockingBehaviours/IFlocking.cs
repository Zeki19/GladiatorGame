using System.Collections.Generic;
using UnityEngine;
public interface IFlocking
{
    Vector2 GetDir(List<IBoid> boids, IBoid self);
    public bool IsActive { get; set; }
}
