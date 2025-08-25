using UnityEngine;

public class Pillar_OBS : MonoBehaviour
{
    public enum PillarState { Standing, Fallen, Destroyed }
    private PillarState state = PillarState.Standing;

    //paceholder
    [Header("Durability")]
    public float health = 100f;         
    public float damageToFall = 50f;     
    public float damageToDestroy = 100f; 
    //

    [Header("Fallen Pillar Settings")]
    public float fallAoERadius = 3f;
    public float knockbackForce = 6f; 
    public GameObject fallenVisual;   
    public GameObject standingVisual; 

    [Header("Destroyed Pillar Settings")]
    public GameObject debrisVisual;   
    public float movementSlowFactor = 0.7f;

    [Header("Debug")]
    public bool blockProjectiles = true; 

    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        SetState(PillarState.Standing);
    }
    public void TakeDamage(float amount)
    {
        if (state == PillarState.Destroyed) return;

        health -= amount;

        if (state == PillarState.Standing && health <= (damageToFall))
        {
            Fall();
        }
        else if (state == PillarState.Fallen && health <= (damageToDestroy))
        {
            Shatter();
        }
    }
    private void Fall()
    {
        SetState(PillarState.Fallen);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, fallAoERadius);
        foreach (var hit in hits)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 dir = (rb.position - (Vector2)transform.position).normalized;
                rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
    private void Shatter()
    {
        SetState(PillarState.Destroyed);
    }
    private void SetState(PillarState newState)
    {
        state = newState;

        if (standingVisual) standingVisual.SetActive(state == PillarState.Standing);
        if (fallenVisual) fallenVisual.SetActive(state == PillarState.Fallen);
        if (debrisVisual) debrisVisual.SetActive(state == PillarState.Destroyed);

        switch (state)
        {
            case PillarState.Standing:
                blockProjectiles = true;
                if (col) col.enabled = true;
                break;

            case PillarState.Fallen:
                blockProjectiles = false; 
                if (col) col.enabled = true; 
                break;

            case PillarState.Destroyed:
                blockProjectiles = false;
                if (col) col.enabled = false; 
                break;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (state == PillarState.Fallen)
            Gizmos.DrawWireSphere(transform.position, fallAoERadius);
    }
}
