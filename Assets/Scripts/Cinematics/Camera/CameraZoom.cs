using System;
using System.Collections;
using UnityEngine;

public class CameraZoom : CameraModuleBase
{
    Coroutine _co;

    public void ZoomTo(float lensGoal, float duration, Action onEnd = null)
    {
        if (Ctx?.Cam == null) { Debug.LogError("Zoom: no cam"); return; }
        if (_co != null) StopCoroutine(_co);
        _co = StartCoroutine(Run(lensGoal, duration, onEnd));
    }

    public void Stop()
    {
        if (_co != null) StopCoroutine(_co);
    }

    IEnumerator Run(float goal, float duration, Action onEnd)
    {
        var cam = Ctx.Cam;
        bool ortho = cam.Lens.Orthographic;
        float start = ortho ? cam.Lens.OrthographicSize : cam.Lens.FieldOfView;
        float t = 0f; duration = Mathf.Max(0.0001f, duration);

        while (t < duration)
        {
            t += Time.deltaTime;
            float v = Mathf.Lerp(start, goal, t / duration);
            if (ortho) cam.Lens.OrthographicSize = v; else cam.Lens.FieldOfView = v;
            yield return null;
        }
        if (ortho) cam.Lens.OrthographicSize = goal; else cam.Lens.FieldOfView = goal;
        _co = null; onEnd?.Invoke();
    }
}