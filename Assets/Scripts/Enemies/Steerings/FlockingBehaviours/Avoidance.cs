using System.Collections.Generic;
using UnityEngine;

public class Avoidance : MonoBehaviour, IFlocking
{
    [SerializeField] public float multiplier = 1f;
    [SerializeField] public float personalArea = 0.5f;
    public Vector3 GetDir(List<IBoid> boids, IBoid self)
    {
        Vector3 avoidance = Vector3.zero;
        for (int i = 0; i < boids.Count; ++i)
        {
            Vector3 diff = self.Position - boids[i].Position;

            if(diff.magnitude> personalArea) continue;

            avoidance += diff.normalized * (personalArea - diff.magnitude);
        }
        return avoidance.normalized * multiplier;
    }
}
