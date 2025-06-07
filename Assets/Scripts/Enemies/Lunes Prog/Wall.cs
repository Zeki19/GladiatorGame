using UnityEngine;

public class Wall : MonoBehaviour
{
    private WallsManager _wallsManager;
    private void Start()
    {
        var coll = GetComponent<Collider2D>();
        ServiceLocator.Instance.GetService<WallsManager>().AddColl(coll);
    }
}
