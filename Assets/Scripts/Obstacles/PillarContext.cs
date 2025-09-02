using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

public class PillarContext
{
    public Transform Origin;
    public List<Vector3> OccupiedSpaces =  new List<Vector3>();
}

public interface IPillar
{
    event System.Action OnDamaged;
    void SpawnPillar(PillarContext context);
    void DestroyPillar(PillarContext context);
}