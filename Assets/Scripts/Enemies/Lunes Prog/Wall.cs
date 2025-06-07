using UnityEngine;

public class Wall : MonoBehaviour
{
    private WallsManager _wallsManager;
    private void Start()
    {
        _wallsManager = ServiceLocator.Instance.GetService<WallsManager>();
        var coll = GetComponent<Collider2D>();
        
        _wallsManager.AddColl(coll);
    }
    private void OnDestroy()
    {
        var coll = GetComponent<Collider2D>();
        _wallsManager.RemoveColl(coll);
    }
}
