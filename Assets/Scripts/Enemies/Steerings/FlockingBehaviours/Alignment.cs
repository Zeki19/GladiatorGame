using System.Collections.Generic;
using UnityEngine;

public class Alignment : FlockingBaseBehaviour
{
    protected override Vector2 GetRealDir(List<IBoid> boids, IBoid self)
    {
        Vector2 alignment = Vector2.zero;

        for (int i = 0; i < boids.Count; i++)
        {
            var currBoid = boids[i];
            alignment += new Vector2(currBoid.Forward.x, currBoid.Forward.y);
        }
        return alignment.normalized * multiplier;
    }
}
