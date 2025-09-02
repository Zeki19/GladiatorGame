using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pillar_Shatter : MonoBehaviour, IPillar
{
    [SerializeField] private string paintTag = "Scraps";
    
    private ArenaPainter _painter;

    public void StartSpawn(PillarContext context)
    {
        _painter = ServiceLocator.Instance.GetService<ArenaPainter>();
        
        foreach (var p in context.OccupiedSpaces)
        {
            _painter.PaintArenaNoRotation(p, paintTag);
            _painter.PaintArenaNoRotation(p + GetRandomCardinal(), paintTag);
        }
        
        gameObject.SetActive(false);
    }
    
    private static Vector3 GetRandomCardinal()
    {
        int r = Random.Range(0, 4);
        switch (r)
        {
            case 0: return Vector3.right;
            case 1: return Vector3.left;
            case 2: return Vector3.up;
            default: return Vector3.down;
        }
    }
}
