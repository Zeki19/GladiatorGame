using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshService : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;
    private void Awake()
    {
        ServiceLocator.Instance.RegisterService(this);
    }

    public void RebuildNavMesh()
    {
        surface.RemoveData();
        surface.BuildNavMesh();
    }
}
