using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class ZoomCameraCommand : ICameraCommand
{
    private readonly CinemachineCamera _camera;
    private readonly float _targetZoom;
    private readonly float _duration;

    public ZoomCameraCommand(CinemachineCamera camera, float targetZoom, float duration = 1f)
    {
        _camera = camera;
        _targetZoom = targetZoom;
        _duration = duration;
    }

    public IEnumerator Execute()
    {
        float startZoom = _camera.Lens.OrthographicSize;
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _duration;
            _camera.Lens.OrthographicSize = Mathf.Lerp(startZoom, _targetZoom, t);
            yield return null;
        }

        _camera.Lens.OrthographicSize = _targetZoom;
    }
}