using UnityEngine;

public class ToPoint : ISteering
{
    private readonly Vector2 _destination;
    private readonly Transform _currentPosition;
    public ToPoint(Vector2 destination, Transform currentPosition)
    {
        _destination = destination;
        _currentPosition = currentPosition;
    }
    public Vector2 GetDir()
    {
        return (_destination - (Vector2)_currentPosition.position).normalized;
    }
}
