using UnityEngine;

public class MyComponent : MonoBehaviour
{
    // Foldout states
    public bool showGroupA = true;
    public bool showGroupB = false;

    // Regular (always visible) fields
    public string someName;
    public int someID;

    // Group A floats
    public float a1, a2;

    // Group B floats
    public float b1, b2;
}
