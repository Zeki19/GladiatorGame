using UnityEngine;

[System.Serializable]
public class CameraEvent
{
    public string eventName;
    public Transform target;
    public float moveDuration = 2f;
    public bool shouldZoom = false;
    public float zoomAmount = 5f;
    public float zoomDuration = 1f;
}