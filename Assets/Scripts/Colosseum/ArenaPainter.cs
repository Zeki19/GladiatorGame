using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;


public class ArenaPainter : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap figthingArenaTilemap;
    [SerializeField] private Tilemap bloodTilemap;

    [Header("Possible types of tiles")]
    [SerializeField] private List<TileType> tileTypes;

    [Header("Testing")] 
    [SerializeField] private Transform player;
    [SerializeField] private String _type;
    
    void Start()
    {
        ServiceLocator.Instance.RegisterService(this);
        InvokeRepeating(nameof(Try), 0f, 3f);
    }

    private void Try()
    {
        PaintArena(player, _type);
    }

    public void PaintArena(Transform pos, String effectName)
    {
        var cell = grid.WorldToCell(pos.position);
        
        var tile = CreateTile(GetCorrectType(effectName));
        
        bloodTilemap.SetTile(cell, figthingArenaTilemap.HasTile(cell) ? tile : null);
    }

    private TileType GetCorrectType(String tileName)
    {
        foreach (var type in tileTypes)
        {
            if (type.name == tileName)
            {
                return type;
            }
        }

        return null;
    }
    
    private static Tile CreateTile(TileType type)
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = type.sprite;
        tile.color = type.color;
        tile.name = type.name;
        return tile;
    }
}

