using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HoundsCamp : MonoBehaviour
{
    [SerializeField] private float campRadius;
    [SerializeField] private float limitMultiplier;
    
    public Vector2 CampCenter => transform.position;
    
    public bool IsFarFromCamp(Vector2 position)
    {
        return Vector2.Distance(position, CampCenter) > (limitMultiplier * campRadius);
    }

    public Vector2 GetRandomPatrolPoint() //Nice to Upgrade
    {
        Vector2 randomCircle = Random.insideUnitCircle * (campRadius);
        return CampCenter + randomCircle;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(CampCenter, campRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CampCenter, campRadius * limitMultiplier );
    }
}
