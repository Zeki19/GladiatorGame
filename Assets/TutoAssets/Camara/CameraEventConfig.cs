using UnityEngine;

[System.Serializable]
public class CameraEventConfig
{
    public string eventId;
    public string targetTag = "";           // Tag para encontrar el target
    public string targetName = "";          // Nombre del GameObject
    public float moveDuration = 2f;
    public bool shouldZoom = false;
    public float zoomAmount = 5f;
    public float zoomDuration = 1f;
    public bool executeAfterDialogue = false; // Si true, se ejecuta después del diálogo
}