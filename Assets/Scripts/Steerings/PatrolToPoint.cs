using UnityEngine;

public class PatrolToPoint : ISteering
{
    private readonly Vector2 _destination;
    private readonly Vector2 _currentPosition;
    public PatrolToPoint(Vector2 destination, Vector2 currentPosition)
    {
        _destination = destination;
        _currentPosition = currentPosition;
    }
    public Vector2 GetDir()
    {
        return (_destination - _currentPosition).normalized;
    }
}
