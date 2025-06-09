using System.Collections.Generic;
using UnityEngine;

public class Cohesion : FlockingBaseBehaviour
{
    protected override Vector2 GetRealDir(List<IBoid> boids, IBoid self)
    {
        Vector2 cohesion = Vector2.zero;
        Vector2 center = Vector2.zero;

        for (int i = 0; i< boids.Count; i++)
        {
            center += new Vector2(boids[i].Position.x, boids[i].Position.y);
        }

        if (boids.Count > 0) 
        { 
            center /= boids.Count;
            cohesion = center - new Vector2(self.Position.x, self.Position.y);
        }

        return cohesion * multiplier;
    }
}
