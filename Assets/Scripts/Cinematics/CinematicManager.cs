using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera camera;
    [SerializeField] private Transform player;
    [SerializeField] private Transform boss;
    
    public DialogueManager dialogueManager;
    public DialogueSO dialogue;
    
    private UIManager _uiManager;
    
    private float _zoomPlayer = 4.92f;
    private float _zoomEnemy = 10;
    
    private float _zoomSpeed = 1.2f;

    private Action _onZoomEnd;
    private Coroutine _frameRoutine;
    private void Start()
    {
        _uiManager = ServiceLocator.Instance.GetService<UIManager>();
        
        PauseManager.TogglePauseCinematic();
        _uiManager.HideUI();
        InitialView();
        dialogueManager.OnConversationEnd += PlayerMoment;
        
        StartCoroutine(BeginSequence());
    }
    
    private IEnumerator BeginSequence()
    {
        yield return new WaitForSeconds(1f);
        _onZoomEnd += EnemyMoment;
        Frame(boss, _zoomEnemy);
    }

    private void InitialView()
    {
        camera.Follow = this.gameObject.transform;
        camera.Lens.OrthographicSize = 12f;
    }
    
    void EnemyMoment()
    {
        dialogueManager.StartConversation(dialogue);
        _onZoomEnd -= EnemyMoment;
    }
    
    void PlayerMoment()
    {
        _onZoomEnd += Finished;
        Frame(player, _zoomPlayer);
    }

    void Finished()
    {
        _uiManager.ShowUI();
        
        camera.Follow = player;
        camera.Lens.OrthographicSize = _zoomPlayer;
        
        PauseManager.TogglePauseCinematic();
        
        _onZoomEnd -= Finished;
    }
    
    void Frame(Transform target, float goal)
    {
        if (_frameRoutine != null)
            StopCoroutine(_frameRoutine);

        _frameRoutine = StartCoroutine(FrameRoutine(target, goal));
    }
    
    private IEnumerator FrameRoutine(Transform target, float goalLens)
    {
        // Lerp THIS object towards target while lerping lens size toward goal
        while (true)
        {
            // Move this transform (camera follows this object during the shot)
            transform.position = Vector3.Lerp(
                transform.position,
                target.position,
                Time.unscaledDeltaTime * _zoomSpeed
            );
            
            var lens = camera.Lens;
            lens.OrthographicSize = Mathf.Lerp(
                lens.OrthographicSize,
                goalLens,
                Time.unscaledDeltaTime * _zoomSpeed
            );
            camera.Lens = lens;

            bool posDone = Vector3.Distance(transform.position, target.position) <= 0.02f;
            bool zoomDone = Mathf.Abs(camera.Lens.OrthographicSize - goalLens) <= 0.02f;

            if (posDone && zoomDone)
                break;

            yield return null;
        }
        
        transform.position = target.position;
        var finalLens = camera.Lens;
        finalLens.OrthographicSize = goalLens;
        camera.Lens = finalLens;

        _frameRoutine = null;
        _onZoomEnd?.Invoke();
    }
}

