using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CinematicManager))]
public class CinematicIntro : MonoBehaviour
{
    [Header("Introduction")]
    [SerializeField] private Transform ArenaCenter;
    
    [Header("Dialogue")]
    [SerializeField] private DialogueSO dialogue;
    
    private CinematicManager _cine;
    private Coroutine _co;
    
    private void Awake()
    {
        _cine = GetComponent<CinematicManager>();
        _cine.OnIntro += StartCinematic;
    }
    private void StartCinematic()
    {
        PauseManager.SetPausedCinematic(true);
        _cine.UIManager.HideUI();

        _cine.cam.Lens.OrthographicSize = 7f;

        GoToEnemy();
    }
    private void GoToEnemy()
    {
        _cine.MoveTo(_cine.boss,1.25f, ZoomEnemy);
    }
    private void ZoomEnemy()
    {
        _cine.ZoomTo(5f,.5f, StartDialogue);
    }
    private void StartDialogue()
    {
        _cine.dialogueManager.OnConversationEnd += MoveToPlayer;
        _cine.dialogueManager.StartConversation(dialogue);
    }
    private void MoveToPlayer()
    {
        _cine.MoveTo(_cine.player, 1f, EndCinematic);
        _cine.ZoomTo(_cine.baseZoom, 1f);
    }
    private void EndCinematic()
    {
        _cine.UIManager.ShowUI();
        _cine.OnEndCinematic?.Invoke();
    }
}