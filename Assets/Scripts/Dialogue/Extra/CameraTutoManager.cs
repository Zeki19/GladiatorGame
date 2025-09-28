using System.Collections;
using UnityEngine;
using System;
using Unity.Cinemachine;

public class CameraTutoManager : MonoBehaviour
{
    [Header("Camera settings")]
    [SerializeField] private CinemachineCamera camera;
    [SerializeField] private float zoom;
    [SerializeField] private float zoomSpeed;
    private float _originalZoom;

    private GameObject _player;
    public CameraHelper cameraHelper;
    public GameObject target;
    
    [Header("Dialogue settings")]
    public DialogueManager dialogueManager;
    
    private void Start()
    {
        _originalZoom = camera.Lens.OrthographicSize;
        _player = camera.Follow.gameObject;
        cameraHelper.gameObject.transform.position = _player.transform.position;
        //StartCoroutine(DelayStart());
    }
    
    IEnumerator DelayStart()
    {
        camera.Follow = cameraHelper.gameObject.transform;
        yield return new WaitForSeconds(3f);
        
        cameraHelper.MoveToTarget(target.transform);
        dialogueManager.StartConversation(EnumDialogues.Mission0);
        Zoom(EnumZoom.In);
        dialogueManager.OnConversationEnd += TutorialEnded;
    }

    private void Zoom(EnumZoom direction)
    {
        float targetZoom = direction switch
        {
            EnumZoom.In => zoom,
            EnumZoom.Out => _originalZoom,
            _ => camera.Lens.OrthographicSize
        };

        StopAllCoroutines();
        StartCoroutine(ZoomTo(targetZoom));
    }
    private IEnumerator ZoomTo(float targetZoom)
    {
        float startZoom = camera.Lens.OrthographicSize;
        float elapsedTime = 0f;
        float duration = Mathf.Abs(startZoom - targetZoom) / zoomSpeed;

        while (elapsedTime < duration)
        {
            camera.Lens.OrthographicSize = Mathf.Lerp(startZoom, targetZoom, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        camera.Lens.OrthographicSize = targetZoom;
    }
    
    void TutorialEnded()
    {
        Zoom(EnumZoom.Out);
        cameraHelper.MoveToTarget(_player.transform);
    }
}

public enum EnumZoom
{
    In = 0,
    Out = 1,
}
