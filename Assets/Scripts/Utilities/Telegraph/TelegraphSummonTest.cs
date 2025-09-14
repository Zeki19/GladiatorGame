using UnityEngine;

public class TelegraphSummonTest : MonoBehaviour
{
    [SerializeField] GameObject telegraphTriangle;
    [SerializeField] GameObject telegraphSquare;
    [SerializeField] GameObject telegraphCircle;

    [ContextMenu("ShowTriangle")]
    void PerformTriangleAttack()
    {
        GameObject telegraph = Instantiate(telegraphTriangle, transform.position, Quaternion.identity);
    }

    [ContextMenu("ShowSquare")]
    void PerformSquareAttack()
    {
        GameObject telegraph = Instantiate(telegraphSquare, transform.position, Quaternion.identity);
    }

    [ContextMenu("ShowCircle")]
    void PerformCircleAttack()
    {
        GameObject telegraph = Instantiate(telegraphCircle, transform.position, Quaternion.identity);
    }
}
