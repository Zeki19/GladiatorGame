using System.Collections.Generic;
using UnityEngine;

public class LeaderBehaviour : FlockingBaseBehaviour
{
    [SerializeField] Transform Enemy;
    public float timePrediction;
    StSeek _seek;
    StPursuit _pursuit;
    bool _isPursuit;
    bool _isThereLeader = false;
    private void Awake()
    {
        _pursuit = new StPursuit(transform);
        _seek = new StSeek(transform.position);
    }
    protected override Vector3 GetRealDir(List<IBoid> boids, IBoid self)
    {
        if (!_isThereLeader)
        {
            Leader = Enemy;
        }
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

    public Transform GetLeader(IBoid self, int radius, LayerMask boidMask) 
    {
        Collider2D[] colls = new Collider2D[5];
        List<Transform> transforms = new List<Transform>();

        colls = Physics2D.OverlapCircleAll(self.Position, radius, boidMask);

        Vector3 center = Vector2.zero;
        float minDistance = 0;
        Transform leader = null;
        foreach (Collider2D coll in colls) 
        {
            Transform trans = coll.GetComponent<Transform>();
            transforms.Add(trans);
            center += trans.position;
        }
        foreach (Transform trans in transforms)
        {
            float currDistance = (center - trans.position).magnitude;
            if (currDistance < minDistance || minDistance == 0)
            {
                leader = trans;
                minDistance = currDistance;
            }
        }
        if (leader == transform)
        {
            _isThereLeader = false;
        }
        else
        {
            _isThereLeader = true;
        }

        return leader;
    }
}
