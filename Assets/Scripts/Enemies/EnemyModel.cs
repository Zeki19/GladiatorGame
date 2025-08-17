using System.Collections;
using Enemies;
using Entities;
using UnityEngine;
using UnityEngine.AI;

public class EnemyModel : EntityModel
{
    protected float _speedModifier = 1;
    [SerializeField] float _moveSpeed;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float defaultDashTime = 0.2f;
    public override void Dash(float dashForce)
    {
        Dash(transform.up, dashForce);
    }

    public override void Dash(Vector2 dir, float dashForce)
    {
        StartCoroutine(DashRoutine(dir.normalized, dashForce, defaultDashTime));
    }

    public override void Dash(Vector2 dir, float dashForce, float dashDistance)
    {
        // Use dashDistance instead of dashForce for travel length
        StartCoroutine(DashRoutine(dir.normalized, dashDistance, defaultDashTime));

        // Preserve your monitoring behavior
        var controller = manager.controller as EnemyController;
        controller.StartDashMonitoring(dir.normalized, dashDistance, transform.position);
    }

    private IEnumerator DashRoutine(Vector2 dir, float distance, float dashTime)
    {
        // Stop normal pathfinding
        agent.isStopped = true;

        Vector3 start = transform.position;
        Vector3 end = start + (Vector3)dir.normalized * (distance * 1.1f);

        // Optional: Clamp to NavMesh so we don't dash into non-walkable areas
        if (NavMesh.SamplePosition(end, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            end = hit.position;
        }

        float elapsed = 0f;
        while (elapsed < dashTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / dashTime;

            // Smooth movement without breaking NavMesh
            agent.Warp(Vector3.Lerp(start, end, t));
            yield return null;
        }

        // Resume normal pathfinding
        agent.isStopped = false;
    }

    public override void ModifySpeed(float speed)
    {
        _speedModifier += speed;
    }

    public override void Move(Vector2 dir)
    {
        dir.Normalize();
        if(manager.Rb != null)
        manager.Rb.linearVelocity = dir * (_moveSpeed * _speedModifier);
    }

    
    
}

