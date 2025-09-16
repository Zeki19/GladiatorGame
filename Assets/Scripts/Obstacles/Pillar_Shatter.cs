using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pillar_Shatter : MonoBehaviour, IPillar
{
    [SerializeField] private string paintTag = "Scraps";
    List<Vector3> _occupiedPositions = new List<Vector3>();
    
    private ArenaPainter _painter;

    public event Action OnDamaged;

    public void SpawnPillar(PillarContext context)
    {
        _painter = ServiceLocator.Instance.GetService<ArenaPainter>();
        
        foreach (var p in context.OccupiedSpaces)
        {
            _painter.PaintArenaNoRotation(p, paintTag);
            _occupiedPositions.Add(p);
            
            var newPos = p + GetRandomCardinal();
            _painter.PaintArenaNoRotation(newPos, paintTag);
            _occupiedPositions.Add(newPos);
        }
        
        gameObject.SetActive(false);
    }

    public void DestroyPillar(PillarContext context)
    {
        foreach (var pos in _occupiedPositions)
        {
            _painter.ClearPaint(pos);
        }

        OnDamaged = null;
        
        _occupiedPositions.Clear();
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        OnDamaged?.Invoke();
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
