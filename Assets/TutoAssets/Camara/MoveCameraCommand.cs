using System.Collections;
using UnityEngine;

public class MoveCameraCommand : ICameraCommand
{
    private readonly Transform _target;
    private readonly float _duration;
    private readonly Transform _cameraTransform;
    private readonly float _moveSpeed;

    public MoveCameraCommand(Transform cameraTransform, Transform target, float duration = 2f, float moveSpeed = 5f)
    {
        _cameraTransform = cameraTransform;
        _target = target;
        _duration = duration;
        _moveSpeed = moveSpeed;
    }

    public IEnumerator Execute()
    {
        float elapsed = 0f;
        Vector3 startPos = _cameraTransform.position;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _duration;
            t = Mathf.SmoothStep(0f, 1f, t);

            _cameraTransform.position = Vector3.Lerp(startPos, _target.position, t);
            yield return null;
        }

        _cameraTransform.position = _target.position;
    }
}