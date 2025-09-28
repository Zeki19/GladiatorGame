using UnityEngine;

[System.Serializable]
public class CameraEventConfig
{
    public string eventId;
    public string targetTag = "";           
    public string targetName = "";          
    public float moveDuration = 2f;
    public bool shouldZoom = false;
    public float zoomAmount = 5f;
    public float zoomDuration = 1f;
    public bool executeAfterDialogue = false; 
}