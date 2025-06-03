using System.Collections.Generic;
using UnityEngine;

public class Alignment : MonoBehaviour, IFlocking
{
    [SerializeField] public float multiplier = 1;
    public Vector3 GetDir(List<IBoid> boids, IBoid self)
    {
        Vector3 alignment = Vector3.zero;

        for (int i = 0; i < boids.Count; i++)
        {
            var currBoid = boids[i];
            alignment += currBoid.Forward;
        }

        return alignment.normalized * multiplier;
    }
}
