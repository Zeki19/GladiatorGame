using UnityEngine;

public class CinematicDefeat : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueSO dialogue;
    
    private CinematicManager _cine;
    private Coroutine _co;
    
    private void Awake()
    {
        _cine = GetComponent<CinematicManager>();
        _cine.OnDefeat += StartCinematic;
    }
    private void StartCinematic()
    {
        PauseManager.SetPausedCinematic(true);
        _cine.UIManager.HideUI();
        
        _cine.MoveTo(_cine.boss,.5f);
        _cine.ZoomTo(6f,.75f, StartDialogue);
    }
    
    private void StartDialogue()
    {
        _cine.dialogueManager.OnConversationEnd += FramePlayer;
        _cine.dialogueManager.StartConversation(dialogue);
    }

    private void FramePlayer()
    {
        _cine.MoveTo(_cine.player, 1f);
        _cine.ZoomTo(4f, 1.5f, EndCinematic);
    }
    
    private void EndCinematic()
    {
        ServiceLocator.Instance.GetService<SceneChanger>().ChangeScene("DefeatScene");
    }
}
