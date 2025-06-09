using Enemies;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBehaviour : FlockingBaseBehaviour
{
    [SerializeField] Transform Enemy;
    public float timePrediction;
    StSeek _seek;
    StPursuit _pursuit;
    bool _isPursuit;
    private void Awake()
    {
        _pursuit = new StPursuit(transform, timePrediction);
        _seek = new StSeek(transform.position);
    }
    protected override Vector2 GetRealDir(List<IBoid> boids, IBoid self)
    {
        if (_isPursuit)
        {
            return _pursuit.GetDir() * multiplier;
        }
        return _seek.GetDir() * multiplier;
    }
    public Transform Leader
    {
        set
        {
            var rb = value.GetComponent<Rigidbody2D>();
            if (rb)
            {
                _pursuit.Target = rb;
                _isPursuit = true;
            }
            else
            {
                _seek.Target = value.position;
                _isPursuit = false;
            }
        }
    }
}
