using Player;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Missions/Mission6_DropWeapon", fileName = "Mission6_DropWeapon")]
public class Mission6_DropWeapon : TutorialMission
{
    private bool _hasCompleted = false;

    protected override void OnInitialize()
    {
        missionName = "Drop Weapon";
        missionDescription = "Press Q to drop your weapon";
        dialogueToPlay = EnumDialogues.Mission6;
        _hasCompleted = false;

        PlayerWeaponController.OnPlayerWeaponDropped += OnPlayerWeaponDropped;
    }

    private void OnPlayerWeaponDropped()
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
            Debug.Log("Mission6_DropWeapon: CheckCompletion - Esperando evento...");
        }
    }

    protected override void OnCleanup()
    {
        PlayerWeaponController.OnPlayerWeaponDropped -= OnPlayerWeaponDropped;
    }

    [ContextMenu("Force Complete")]
    public void ForceComplete()
    {
        Debug.Log("Mission6_DropWeapon: FORCE COMPLETE");
        CompleteMission();
    }
}