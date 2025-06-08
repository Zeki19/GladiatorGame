using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HoundsCamp : MonoBehaviour
{
    [SerializeField] public float patrolRadius;
    [SerializeField] public float chaseRadius;

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
    
    public Vector3Int GetRandomPointV3I()
    {
        Vector2 randomOffset = Random.insideUnitCircle * patrolRadius;
        return Vector3Int.RoundToInt(CampCenter + randomOffset);
    }
    
    public List<Vector3Int> GetPoints(int amount)
    {
        var waypoints = new List<Vector3Int>();
        for (var i = 0; i < amount; i++)
        {
            waypoints.Add(GetRandomPointV3I());
        }
        return waypoints;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(CampCenter, patrolRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CampCenter, patrolRadius * chaseRadius );
    }
}
