using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void Start()
    {
        var coll = GetComponent<Collider2D>();
        ObstacleManager.Instance.AddColl(coll);
    }
    private void OnDestroy()
    {
        var coll = GetComponent<Collider2D>();
        ObstacleManager.Instance.RemoveColl(coll);
    }
}
