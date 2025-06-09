using System.Collections.Generic;
using UnityEngine;

public class Avoidance : FlockingBaseBehaviour
{
    [SerializeField] public float personalArea = 0.5f;
    protected override Vector2 GetRealDir(List<IBoid> boids, IBoid self)
    {
        Vector2 avoidance = Vector3.zero;
        for (int i = 0; i < boids.Count; ++i)
        {
            Vector2 diff = self.Position - boids[i].Position;

            if(diff.magnitude > personalArea) continue;

            avoidance += diff.normalized * (personalArea - diff.magnitude);
        }
        return avoidance.normalized * multiplier;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, personalArea);
    }
}
