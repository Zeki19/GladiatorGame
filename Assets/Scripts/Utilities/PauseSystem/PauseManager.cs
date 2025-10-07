using System;
using UnityEngine;

public static class PauseManager
{
    public static bool IsPaused { get; private set; }
    public static event Action<bool> OnPauseStateChanged;

    public static void SetPaused(bool paused)
    {
        if (IsPaused == paused)
            return;

        IsPaused = paused;
        //This can be removed if we want something physics base to work, or we can use Time.UnscaledDeltaTime.
        Time.timeScale = paused ? 0f : 1f;
        OnPauseStateChanged?.Invoke(IsPaused);
    }

    public static void TogglePause()
    {
        SetPaused(!IsPaused);
    }
}
