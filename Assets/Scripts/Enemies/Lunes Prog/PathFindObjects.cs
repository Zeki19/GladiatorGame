using UnityEngine;

public class PathFindObjects : MonoBehaviour
{
    private GridManager _wallsManager;
    [SerializeField] private int weight;
    [SerializeField] private int range;
    private void Start()
    {
        _wallsManager = ServiceLocator.Instance.GetService<GridManager>();
        _wallsManager.AddPickUp(transform.position, weight, range);
    }
    
}