using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;


//Name of the script can be updated.
public class ArenaPainter : MonoBehaviour
{
    [Header("Grid and tile-maps")] [SerializeField]
    private Grid grid;

    [SerializeField] private Tilemap figthingArenaTilemap;
    [SerializeField] private Tilemap bloodTilemap;

    [Header("Possible types of tiles")] [SerializeField]
    private List<TileType> tileTypes;

    private readonly Dictionary<string, Tile> _createdTiles = new();

    void Awake()
    {
        ServiceLocator.Instance.RegisterService(this);
        InitializeTiles();
    }

    public void PaintArena(Transform pos, String effectName, int size = 0)
    {
        var cell = grid.WorldToCell(pos.position);

        var tileType = GetType(effectName);
        if (tileType == null)
        {
            Debug.Log("No tileType with that Name");
            return;
        }

        int index = (size == 0) ? UnityEngine.Random.Range(0, tileType.sprites.Length) : size;


        var tile = _createdTiles[tileType.name + index];

        if (figthingArenaTilemap.HasTile(cell))
        {
            bloodTilemap.SetTile(cell, tile);
            RotateTile(cell);
        }
        else
        {
            bloodTilemap.SetTile(cell, null);
        }
    }

    private void InitializeTiles()
    {
        foreach (var t in tileTypes)
        {
            for (int i = 0; i < t.sprites.Length; i++)
            {
                var tile = CreateTile(t, i);
                _createdTiles.Add(t.name + i, tile);
            }
        }
    }
    private TileType GetType(String tileName)
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
    private static Tile CreateTile(TileType type, int index)
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = type.sprites[index];
        tile.color = type.color;
        tile.name = type.name + index;
        return tile;
    }
    private void RotateTile(Vector3Int cell)
    {
        var randomRotation = UnityEngine.Random.Range(0, 360);
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, randomRotation));
        bloodTilemap.SetTransformMatrix(cell, rotationMatrix);
    }
    private void CleanArena()
    {
        bloodTilemap.ClearAllTiles();
    }

}
