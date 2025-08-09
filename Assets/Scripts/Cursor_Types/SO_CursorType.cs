using UnityEngine;
[CreateAssetMenu(menuName ="cursor/newType",fileName = "cursorProfile")]
public class SO_CursorType : ScriptableObject
{
    [Header("cursor Name")]
    public string name;
    [Header("Main cursor textures")]
    public Texture2D mainTexture;
    public Texture2D clickTexture;
    public Texture2D shadowTexture;
    [Header("Main cursor Scale")]
    [Range(0f,1f)] public float cursorScale;
    [Header("Main cursor Options")]
    public Vector2 shadowOffset;
    public Color shadowColour;
}
