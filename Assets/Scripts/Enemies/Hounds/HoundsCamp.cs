using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HoundsCamp : MonoBehaviour
{
    [SerializeField] private float patrolRadius;
    [SerializeField] private float chaseRadius;

    public Vector2 CampCenter => transform.position;
    
    public bool IsFarFromCamp(Vector2 position)
    {
        return Vector2.Distance(position, CampCenter) > patrolRadius * chaseRadius;
    }
    
    public bool IsInCamp(Vector2 position)
    {
        return Vector2.Distance(position, CampCenter) < patrolRadius;
    }


    public Vector2 GetRandomPoint()
    {
        Vector2 randomOffset = Random.insideUnitCircle * patrolRadius;
        return CampCenter + randomOffset;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(CampCenter, patrolRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CampCenter, patrolRadius * chaseRadius );
    }
}
