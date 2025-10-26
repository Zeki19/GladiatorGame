using System;
using System.Collections;
using UnityEngine;

public class CameraMove : CameraModuleBase
{
    Coroutine _co;

    public void MoveTo(Transform target, float duration, Action onEnd = null)
    {
        if (Ctx?.Cam == null || Ctx.Helper == null || target == null) { return; }
        if (_co != null) StopCoroutine(_co);
        _co = StartCoroutine(MoveRoutine(target, duration, onEnd));
    }

    public IEnumerator MoveRoutine(Transform target, float duration, Action onEnd)
    {
        var helper = Ctx.Helper;
        
        helper.position = Ctx.Cam.transform.position; 
        Ctx.Cam.Follow = helper;
        
        Vector3 startPos = helper.position;
        Vector3 endPos = target.position;
        
        float elapsed = 0f;
        duration = Mathf.Max(0.0001f, duration);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            helper.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        
        helper.transform.position = target.position;
        _co = null;
        onEnd?.Invoke();
        onEnd = null;
    }
}
