using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CinematicManager))]
public class CinematicVictory : MonoBehaviour
{
    [Header("Victory")]
    [SerializeField] private Transform exitDoor;
    
    [Header("Dialogue")]
    [SerializeField] private DialogueSO dialogue;
    
    private CinematicManager _cine;
    private Coroutine _co;
    
    private void Awake()
    {
        _cine = GetComponent<CinematicManager>();
    }
    private void Start()
    {
        _cine.OnVictory += StartCinematic;
    }
    private void StartCinematic()
    {
        PauseManager.SetPausedCinematic(true);
        _cine.UIManager.HideUI();
        
        _cine.ZoomTo(6f,1f, StartDialogue);
    }
    private void StartDialogue()
    {
        _cine.dialogueManager.OnConversationEnd += MoveToExit;
        _cine.dialogueManager.StartConversation(dialogue);
    }
    private void MoveToExit()
    {
        _cine.MoveTo(exitDoor, 2.5f, ShowExit);
        _cine.dialogueManager.OnConversationEnd -= MoveToExit;
    }
    private void ShowExit()
    {
        _cine.ZoomTo(4f, 1f, ZoomOutExit);
    }
    private void ZoomOutExit()
    {
        _cine.ZoomTo(_cine.baseZoom, 1f, BackToPlayer);
    }
    private void BackToPlayer()
    {
        _cine.MoveTo(_cine.player, 1.5f, EndCinematic);
    }
    private void EndCinematic()
    {
        _cine.OnEndCinematic?.Invoke();
    }
}
