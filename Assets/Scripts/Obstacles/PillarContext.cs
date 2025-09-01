using System.Collections.Generic;
using UnityEngine;

public class PillarContext
{
    public Transform Origin;
    public List<Vector3> OccupiedSpaces =  new List<Vector3>();
}

public interface IPillar
{
    void StartSpawn(PillarContext context, IHealth healthSystem = null);
}