using Player;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Missions/Mission3_Attack", fileName = "Mission3_Attack")]


public class Mission3_Attack : TutorialMission
{
    private bool _hasCompleted = false;

    protected override void OnInitialize()
    {
        missionName = "Basic Attack";
        missionDescription = "Press the left click to strike me with your blade";
        dialogueToPlay = EnumDialogues.Mission3;
        _hasCompleted = false;

        PlayerWeaponController.OnPlayerAttacked += OnPlayerAttacked;
    }
    private void OnPlayerAttacked()
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
            Debug.Log("Mission3_Attack: CheckCompletion - Esperando evento...");
        }
    }

    protected override void OnCleanup()
    {

        PlayerWeaponController.OnPlayerAttacked -= OnPlayerAttacked;
    }

    [ContextMenu("Force Complete")]
    public void ForceComplete()
    {
        Debug.Log("Mission3_Attack: FORCE COMPLETE");
        CompleteMission();
    }
}
