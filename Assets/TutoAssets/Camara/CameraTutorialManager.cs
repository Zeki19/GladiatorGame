using System.Collections.Generic;
using System;
using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class CameraTutorialManager : MonoBehaviour
{
    [Header("Camera Configuration")]
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private Transform cameraHelper;
    [SerializeField] private float defaultZoom = 10f;

    [Header("Camera Return Speed")]
    [SerializeField] private float returnMoveSpeed = 8f; 

    [Header("Registered Camera Targets")]
    [SerializeField] private List<CameraTarget> registeredTargets = new List<CameraTarget>();

    private Queue<ICameraCommand> _commandQueue = new Queue<ICameraCommand>();
    private Transform _originalTarget;
    private Dictionary<string, Transform> _targetRegistry = new Dictionary<string, Transform>();

    public static event Action<string> OnCameraEventCompleted;

    [System.Serializable]
    public class CameraTarget
    {
        public string id;
        public Transform target;
    }

    private void Awake()
    {
        ServiceLocator.Instance.RegisterService(this);

        if (cameraHelper == null)
        {
            GameObject helperObj = new GameObject("CameraHelper");
            cameraHelper = helperObj.transform;
        }

        foreach (var target in registeredTargets)
        {
            RegisterTarget(target.id, target.target);
        }
    }

    private void Start()
    {
        _originalTarget = cinemachineCamera.Follow;
        defaultZoom = cinemachineCamera.Lens.OrthographicSize;
    }

    public void RegisterTarget(string id, Transform target)
    {
        if (target != null)
        {
            _targetRegistry[id] = target;
            Debug.Log($"Camera target registered: {id}");
        }
    }

    private void RegisterTargetByName(string objectName)
    {
        GameObject obj = GameObject.Find(objectName);
        if (obj != null)
        {
            RegisterTarget(objectName, obj.transform);
        }
    }

    public void ExecuteCameraEvent(CameraEventConfig config, Action onComplete = null)
    {
        Transform target = FindTarget(config);
        if (target == null)
        {
            Debug.LogWarning($"Camera target not found for event: {config.eventId}");
            onComplete?.Invoke();
            return;
        }

        StartCoroutine(ProcessCameraEvent(config, target, onComplete));
    }

    public void ExecuteCameraEvent(string eventId, Action onComplete = null)
    {
        if (_targetRegistry.TryGetValue(eventId, out Transform target))
        {
            CameraEventConfig config = new CameraEventConfig
            {
                eventId = eventId,
                moveDuration = 2f,
                shouldZoom = false
            };

            StartCoroutine(ProcessCameraEvent(config, target, onComplete));
        }
        else
        {
            Debug.LogWarning($"Camera target '{eventId}' not registered");
            onComplete?.Invoke();
        }
    }

    private Transform FindTarget(CameraEventConfig config)
    {
        if (_targetRegistry.TryGetValue(config.eventId, out Transform registeredTarget))
        {
            return registeredTarget;
        }
        if (!string.IsNullOrEmpty(config.targetName))
        {
            GameObject namedObj = GameObject.Find(config.targetName);
            if (namedObj != null) return namedObj.transform;
        }

        return null;
    }

    private IEnumerator ProcessCameraEvent(CameraEventConfig config, Transform target, Action onComplete)
    {
        cinemachineCamera.Follow = cameraHelper;
        cameraHelper.position = cinemachineCamera.transform.position;

        _commandQueue.Enqueue(new MoveCameraCommand(cameraHelper, target, config.moveDuration));

        if (config.shouldZoom)
        {
            _commandQueue.Enqueue(new ZoomCameraCommand(cinemachineCamera, config.zoomAmount, config.zoomDuration));
        }

        while (_commandQueue.Count > 0)
        {
            ICameraCommand command = _commandQueue.Dequeue();
            yield return StartCoroutine(command.Execute());
        }

        OnCameraEventCompleted?.Invoke(config.eventId);
        onComplete?.Invoke();
    }

    public void ResetCamera(Action onComplete = null)
    {
        StartCoroutine(ResetCameraCoroutine(onComplete));
    }

    private IEnumerator ResetCameraCoroutine(Action onComplete)
    {
        yield return StartCoroutine(new ZoomCameraCommand(cinemachineCamera, defaultZoom, 1f).Execute());
        if (_originalTarget != null)
        {
            float returnDuration = 1.2f;
            yield return StartCoroutine(new MoveCameraCommand(cameraHelper, _originalTarget, returnDuration, returnMoveSpeed).Execute());
        }

        cinemachineCamera.Follow = _originalTarget;

        onComplete?.Invoke();
    }

    public void SetReturnMoveSpeed(float speed)
    {
        returnMoveSpeed = speed;
    }
}