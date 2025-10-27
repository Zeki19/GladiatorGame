using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CameraZoom))]
[RequireComponent(typeof(CameraMove))]
public class CinematicManager : MonoBehaviour
{
    [Header("Wire")] 
    [SerializeField] public Transform player;
    [SerializeField] public Transform boss;
    [Space] 
    [SerializeField] public CinemachineCamera cam;
    [Space] 
    [SerializeField] Transform helper;

    [Header("Wire Dialogue")] [SerializeField]
    public DialogueManager dialogueManager;

    [Header("Config")] [SerializeField] public float baseZoom = 4.92f;

    [NonSerialized] public UIManager UIManager;
    private CameraZoom _zoom;
    private CameraMove _move;

    public Action OnEndCinematic;
    public Action OnIntro;
    public Action OnVictory;
    public Action OnDefeat;

    public void Initialize()
    {
        UIManager = ServiceLocator.Instance.GetService<UIManager>();

        _zoom = GetComponent<CameraZoom>();
        _move = GetComponent<CameraMove>();

        var ctx = new CameraContext(cam, helper);
        foreach (var mod in GetComponentsInChildren<ICameraModule>(true))
            mod.Init(ctx);

        OnEndCinematic += End;
    }

    public void ZoomTo(float goal, float duration, Action onEnd = null) => _zoom.ZoomTo(goal, duration, onEnd);
    public void MoveTo(Transform target, float duration, Action onEnd = null) => _move.MoveTo(target, duration, onEnd);

    public void IntroCinematic()
    {
        OnIntro?.Invoke();
    }
    public void DefeatCinematic()
    {
        OnDefeat?.Invoke();
    }

    public void VictoryCinematic()
    {
        OnVictory?.Invoke();
    }

    private void End()
    {
        cam.Follow = player;
        PauseManager.SetPausedCinematic(false);
    }
}
