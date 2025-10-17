using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "New Cursor / CursorProfile", fileName = "CursorProfile")]
public class CursorProfileSO : ScriptableObject
{
    [Header("Profile name")]
    public String name;
    
    [Header("Main cursor textures")]
    public Texture2D mainTexture;
    public Texture2D secondaryTexture;
    
    [Header("Cursor scale")]
    [Range(0f,1f)] public float cursorScale = 0.6f;

    [Header("Shadow")]
    public Color shadowColor = new Color(0,0,0, 0.5f);
    public Vector2 shadowOffset = new Vector2(-15, -15);
}

