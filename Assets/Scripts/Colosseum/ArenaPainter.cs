using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class ArenaPainter : MonoBehaviour
{
    [Header("Grid and tile-maps")]
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap figthingArenaTilemap;
    [SerializeField] private Tilemap bloodTilemap;

    [Header("Possible types of tiles")]
    [SerializeField] private List<TileType> tileTypes;

    private readonly Dictionary<string, Tile> _createdTiles = new();

    void Awake()
    {
        ServiceLocator.Instance.RegisterService(this);
        InitializeTiles();
    }

    //private void OnDestroy()
    //{
    //    ServiceLocator.Instance.RemoveService(this);
    //}

    public void PaintArena(Transform pos, String effectName, int size = 0)
    {
        if (pos == null)
        {
            Debug.LogWarning("ArenaPainter: Transform is null, skipping paint");
            return;
        }
        PaintArena(pos.position, effectName, size);
    }

    public void PaintArena(Vector3 pos, String effectName, int size = 0)
    {
        if (!ValidateComponents())
        {
            Debug.LogWarning("ArenaPainter: Grid or Tilemaps are destroyed/null, skipping paint");
            return;
        }

        var cell = grid.WorldToCell(pos);
        var tileType = GetType(effectName);

        if (tileType == null)
        {
            Debug.LogWarning($"ArenaPainter: No tileType with name '{effectName}'");
            return;
        }

        int index = (size == 0) ? UnityEngine.Random.Range(0, tileType.sprites.Length) : size;

        string tileKey = tileType.name + index;
        if (!_createdTiles.ContainsKey(tileKey))
        {
            Debug.LogWarning($"ArenaPainter: Tile '{tileKey}' not found in dictionary");
            return;
        }

        var tile = _createdTiles[tileKey];

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
        if (!ValidateComponents()) return;

        var randomRotation = UnityEngine.Random.Range(0, 360);
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, randomRotation));
        bloodTilemap.SetTransformMatrix(cell, rotationMatrix);
    }

    private void CleanArena()
    {
        if (!ValidateComponents()) return;
        bloodTilemap.ClearAllTiles();
    }

    public void ClearPaint(Vector3 pos)
    {
        if (!ValidateComponents()) return;

        var cell = grid.WorldToCell(pos);
        bloodTilemap.SetTile(cell, null);
    }

    public void PaintArenaNoRotation(Vector3 pos, String effectName, int size = 0)
    {
        if (!ValidateComponents())
        {
            Debug.LogWarning("ArenaPainter: Grid or Tilemaps are destroyed/null, skipping paint");
            return;
        }

        var cell = grid.WorldToCell(pos);
        var tileType = GetType(effectName);

        if (tileType == null)
        {
            Debug.LogWarning($"ArenaPainter: No tileType with name '{effectName}'");
            return;
        }

        int index = (size == 0) ? UnityEngine.Random.Range(0, tileType.sprites.Length) : size;

        string tileKey = tileType.name + index;
        if (!_createdTiles.ContainsKey(tileKey))
        {
            Debug.LogWarning($"ArenaPainter: Tile '{tileKey}' not found in dictionary");
            return;
        }

        var tile = _createdTiles[tileKey];
        bloodTilemap.SetTile(cell, figthingArenaTilemap.HasTile(cell) ? tile : null);
    }
    private bool ValidateComponents()
    {
        if (grid == null)
        {
            Debug.LogWarning("ArenaPainter: Grid reference is null");
            return false;
        }

        if (figthingArenaTilemap == null)
        {
            Debug.LogWarning("ArenaPainter: Fighting arena tilemap reference is null");
            return false;
        }

        if (bloodTilemap == null)
        {
            Debug.LogWarning("ArenaPainter: Blood tilemap reference is null");
            return false;
        }

        try
        {
            _ = grid.transform;
            _ = figthingArenaTilemap.transform;
            _ = bloodTilemap.transform;
        }
        catch (MissingReferenceException)
        {
            Debug.LogWarning("ArenaPainter: One or more components have been destroyed");
            return false;
        }

        return true;
    }
}
