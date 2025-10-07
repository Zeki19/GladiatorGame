using System;
using UnityEngine;

public static class PauseManager
{
    public static bool IsPaused { get; private set; }
    public static bool IsPausedCinematic { get; private set; }

    public static event Action<bool> OnPauseStateChanged;
    public static event Action<bool> OnCinematicStateChanged;
    public static event Action<bool> OnAnyPauseStateChanged;

    public static void SetPaused(bool paused)
    {
        if (IsPaused == paused)
            return;

        IsPaused = paused;
        UpdateTimeScale();
        OnPauseStateChanged?.Invoke(IsPaused);
    }

    public static void TogglePause()
    {
        SetPaused(!IsPaused);
    }

    public static void SetPausedCinematic(bool paused)
    {
        if (IsPausedCinematic == paused)
            return;

        IsPausedCinematic = paused;
        UpdateTimeScale();
        OnCinematicStateChanged?.Invoke(IsPausedCinematic);
    }

    public static void TogglePauseCinematic()
    {
        SetPausedCinematic(!IsPausedCinematic);
    }

    private static void UpdateTimeScale()
    {
        bool isAnyPaused = IsPaused || IsPausedCinematic;
        //This can be removed if we want something physics base to work, or we can use Time.UnscaledDeltaTime.
        Time.timeScale = isAnyPaused ? 0f : 1f;
        OnAnyPauseStateChanged?.Invoke(isAnyPaused);
    }
}
