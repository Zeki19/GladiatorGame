using UnityEngine;

public class Pillar_Standing : MonoBehaviour, IPillar
{
    public void StartSpawn(PillarContext context)
    {
        transform.position = context.Origin.position;
    }
}
