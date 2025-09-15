using UnityEngine;

public class TelegraphSummonTest : MonoBehaviour
{
    [SerializeField] EnemyTelegraph EnemyTelegraph;

    private void OnEnable()
    {
        if (!EnemyTelegraph)
        {
            Debug.LogWarning("EnemyTelegraph on " + gameObject.name + " is NULL.");
        }
    }

    [ContextMenu("ShowTriangle")]
    void PerformTriangleAttack()
    {
        EnemyTelegraph.InstantiateTelegraph(transform.position, transform.rotation, "TriangleAttack");
    }

    [ContextMenu("ShowSquare")]
    void PerformSquareAttack()
    {
        EnemyTelegraph.InstantiateTelegraph(transform.position, transform.rotation, "SquareAttack");
    }

    [ContextMenu("ShowCircle")]
    void PerformCircleAttack()
    {
        EnemyTelegraph.InstantiateTelegraph(transform.position, transform.rotation, "CircleAttack");
    }
}
