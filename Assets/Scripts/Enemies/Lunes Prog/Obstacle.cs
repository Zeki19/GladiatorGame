using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private ObstacleManager obsManager;
    private void Start()
    {
        var coll = GetComponent<Collider2D>();
        obsManager = ServiceLocator.Instance.GetService<ObstacleManager>();
        obsManager.AddColl(coll);
    }
    private void OnDestroy()
    {
        var coll = GetComponent<Collider2D>();
        obsManager.RemoveColl(coll);
    }
}
