using UnityEngine;

[CreateAssetMenu(fileName = "DummyStats", menuName = "Enemies/Dummy/DummyStats")]
public class DummyStatsSO : ScriptableObject
{
    [Header("Health Settings")]
    public float maxHealth = 100f;

    [Header("Visual Settings")]
    public float damageBlinkDuration = 0.2f;

    [Header("Debug Settings")]
    public bool showDebugInfo = true;

    // El dummy no necesita rangos de ataque, velocidades, etc.
    // Solo necesita stats básicos para funcionar como target de práctica
}