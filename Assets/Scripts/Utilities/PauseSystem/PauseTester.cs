using UnityEngine;

public class PauseTester : MonoBehaviour
{
    [ContextMenu("Gameplay/Toggle Gameplay Pause")]
    private void ToggleGameplayPause()
    {
        PauseManager.TogglePause();
    }

    [ContextMenu("Gameplay/Set Gameplay Pause ON")]
    private void SetGameplayPauseOn()
    {
        PauseManager.SetPaused(true);
    }

    [ContextMenu("Gameplay/Set Gameplay Pause OFF")]
    private void SetGameplayPauseOff()
    {
        PauseManager.SetPaused(false);
    }

    [ContextMenu("Cinematic/Toggle Cinematic Pause")]
    private void ToggleCinematicPause()
    {
        PauseManager.TogglePauseCinematic();
    }

    [ContextMenu("Cinematic/Set Cinematic Pause ON")]
    private void SetCinematicPauseOn()
    {
        PauseManager.SetPausedCinematic(true);
    }

    [ContextMenu("Cinematic/Set Cinematic Pause OFF")]
    private void SetCinematicPauseOff()
    {
        PauseManager.SetPausedCinematic(false);
    }

    [ContextMenu("Log Current Pause States")]
    private void LogStates()
    {
        Debug.Log($"[PauseTester] GameplayPaused: {PauseManager.IsPaused}, CinematicPaused: {PauseManager.IsPausedCinematic}, Time.timeScale: {Time.timeScale}");
    }
}
