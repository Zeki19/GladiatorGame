using UnityEngine;

public class DashStateData : IStateData
{
    public Vector2 Direction { get; }
    public float Force { get; }
    public float Distance { get; }

    public DashStateData(Vector2 direction, float force, float distance)
    {
        Direction = direction;
        Force = force;
        Distance = distance;
    }
}