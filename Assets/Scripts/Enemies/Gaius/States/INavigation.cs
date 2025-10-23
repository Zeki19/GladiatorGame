using UnityEngine.AI;

public interface INavigation
    {
        public NavMeshAgent _NVagent { get; }
        public void TurnOffNavMesh();
        public void RepositionInNavMesh();
    }
