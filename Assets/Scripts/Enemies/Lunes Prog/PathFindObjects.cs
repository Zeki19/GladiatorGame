using UnityEngine;

public class PathFindObjects : MonoBehaviour
{
    private WallsManager _wallsManager;
    [SerializeField] private int value;
    private void Start()
    {
        _wallsManager = ServiceLocator.Instance.GetService<WallsManager>();
        var coll = GetComponent<Collider2D>();
        
        _wallsManager.PickupColl(coll,value);
    }
    
}