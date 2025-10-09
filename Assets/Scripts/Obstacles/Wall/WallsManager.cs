using UnityEngine;

public class WallsManager : MonoBehaviour
{
    [SerializeField] private WallObstacle[] _wallObstacles; 
    void Start()
    {
        for (int i = 0; i < _wallObstacles.Length; i++)
        {
            _wallObstacles[i].SpawnCurrentState();
        }
        ServiceLocator.Instance.GetService<NavMeshService>().RebuildNavMesh();
    }
}
