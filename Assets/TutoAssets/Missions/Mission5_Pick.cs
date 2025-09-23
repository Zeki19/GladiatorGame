using Player;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Missions/Mission5_Pick", fileName = "Mission5_Pick")]

public class Mission5_Pick : TutorialMission
{
    private bool _hasCompleted = false;

    protected override void OnInitialize()
    {
        missionName = "Learn To Grab a Weapon";
        missionDescription = "Press E to grab a weapon";
        dialogueToPlay = EnumDialogues.Mission5;
        _hasCompleted = false;

        PlayerWeaponController.OnPlayerWeaponPicked += OnPlayerPicked;
    }
    private void OnPlayerPicked()
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
            Debug.Log("Mission5_Pick: CheckCompletion - Esperando evento...");
        }
    }

    protected override void OnCleanup()
    {

        PlayerWeaponController.OnPlayerWeaponPicked -= OnPlayerPicked;
    }

    [ContextMenu("Force Complete")]
    public void ForceComplete()
    {
        Debug.Log("Mission5_Pick: FORCE COMPLETE");
        CompleteMission();
    }
}
