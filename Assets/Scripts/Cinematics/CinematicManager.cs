using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
    [Header("Scene objects")]
    [SerializeField] private CinemachineCamera camera;
    [SerializeField] private Transform player;
    [SerializeField] private Transform boss;

    [Header("Dialogue settings")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogueSO dialogue;

    [Header("Zoom settings")]
    [SerializeField] private float zoomSpeed;
    private const float ZoomEnemy = 6.5f;
    private const float ZoomPlayer = 4.92f;

    private UIManager _uiManager;
    private Action _onZoomEnd;
    private Coroutine _frameRoutine;

    private void Start()
    {
        _uiManager = ServiceLocator.Instance.GetService<UIManager>();

        _uiManager.HideUI();
        InitialView();
        dialogueManager.OnConversationEnd += PlayerMoment;

        PauseManager.SetPausedCinematic(true);
        _onZoomEnd += EnemyMoment;
        Frame(boss, ZoomEnemy);
    }

    private void InitialView()
    {
        camera.Follow = transform;
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
        Frame(player, ZoomPlayer);
    }

    void Finished()
    {
        _uiManager.ShowUI();

        camera.Follow = player;

        camera.Lens.OrthographicSize = ZoomPlayer;

        PauseManager.SetPausedCinematic(false);

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
        while (true)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                target.position,
                Time.unscaledDeltaTime * zoomSpeed
            );

            var lens = camera.Lens;
            lens.OrthographicSize = Mathf.Lerp(
                lens.OrthographicSize,
                goalLens,
                Time.unscaledDeltaTime * zoomSpeed
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

    public void SkipCinematic()
    {
        dialogueManager.EndDialogue();
    }
}

