using Player;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Missions/Mission1_Dash", fileName = "Mission1_Dash")]

public class Mission1_Dash : TutorialMission
{
    private bool _hasCompleted = false;

    protected override void OnInitialize()
    {
        missionName = "Learn To Dash";
        missionDescription = "Press SPACE to dash in all directions";
        dialogueToPlay = EnumDialogues.Mission1;
        _hasCompleted = false;

        PlayerModel.OnPlayerDashed += OnPlayerDashed;
    }
    private void OnPlayerDashed()
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
            LogWaitingOnce("Mission1_Dash: CheckCompletion - Esperando evento...");
        }
    }

    protected override void OnCleanup()
    {

        PlayerModel.OnPlayerDashed -= OnPlayerDashed;
    }

    [ContextMenu("Force Complete")]
    public void ForceComplete()
    {
        Debug.Log("Mission1_Dash: FORCE COMPLETE");
        CompleteMission();
    }
}
