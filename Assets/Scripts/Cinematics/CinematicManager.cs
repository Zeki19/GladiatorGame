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
    [SerializeField] private GameObject skipButton;

    [NonSerialized] public UIManager UIManager;
    private CameraZoom _zoom;
    private CameraMove _move;

    public Action OnEndCinematic;
    public Action OnIntro;
    public Action OnVictory;
    public Action OnDefeat;
    
    private bool _introCinematic = false;
    private bool _defeatCinematic = false;

    public void Initialize()
    { 
        _introCinematic = false; 
        _defeatCinematic = false;
    
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
        _introCinematic = true;
        _defeatCinematic = false;
        
        OnIntro?.Invoke();
        skipButton.SetActive(true);
    }
    public void DefeatCinematic()
    {
        _introCinematic = false;
        _defeatCinematic = true;
        
        OnDefeat?.Invoke();
        skipButton.SetActive(true);
    }
    public void VictoryCinematic()
    {
        _introCinematic = false;
        _defeatCinematic = false;
        
        OnVictory?.Invoke();
        skipButton.SetActive(true);
    }
    
    public void SkipCinematic()
    {
        dialogueManager.SkipDialogue();
        
        _zoom.Stop();
        _move.Stop();
        
        _zoom.ZoomTo(baseZoom, 0f);
        
        if (_introCinematic) UIManager.ShowUI();
        if (_defeatCinematic) SceneChanger.Instance.ChangeScene("DefeatScene");
        
        End();
    }
    
    private void End()
    {
        skipButton.SetActive(false);
        cam.Follow = player;
        PauseManager.SetPausedCinematic(false);
        
        _introCinematic = false;
        _defeatCinematic = false;
    }
}
