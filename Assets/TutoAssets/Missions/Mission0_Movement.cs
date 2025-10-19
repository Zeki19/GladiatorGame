using UnityEngine;
using Player;

[CreateAssetMenu(menuName = "Tutorial/Missions/Mission0_Movement", fileName = "Mission0_Movement")]
public class Mission0_Movement : TutorialMission
{
    private bool _hasCompleted = false;

    protected override void OnInitialize()
    {
        missionName = "Learn To Move";
        missionDescription = "Use WASD to move character";
        dialogueToPlay = EnumDialogues.Mission0;
        _hasCompleted = false;


        PlayerModel.OnPlayerMoved += OnPlayerMoved;
    }

    private void OnPlayerMoved()
    {

        if (!_hasCompleted)
        {
            _hasCompleted = true;
            CompleteMission();
        }
    }

    protected override void CheckCompletion()
    {
        if (!_isCompleted)
        {
            LogWaitingOnce("Mission0_Movement: CheckCompletion - Esperando evento...");
        }
    }

    protected override void OnCleanup()
    {

        PlayerModel.OnPlayerMoved -= OnPlayerMoved;
    }

    [ContextMenu("Force Complete")]
    public void ForceComplete()
    {
        Debug.Log("Mission0_Movement: FORCE COMPLETE");
        CompleteMission();
    }
}