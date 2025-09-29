using Player;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Missions/Mission2_Pick", fileName = "Mission2_Pick")]

public class Mission2_Pick : TutorialMission
{
    private bool _hasCompleted = false;

    protected override void OnInitialize()
    {
        missionName = "Learn To Grab a Weapon";
        missionDescription = "Press E to grab a weapon";
        dialogueToPlay = EnumDialogues.Mission2;
        shouldMoveCamera = true;
        cameraEvent = new CameraEventConfig
        {
            eventId = "Weapon",
            targetTag = "Weapon",
            targetName = "SwordRack",
            moveDuration = 0.5f,
            shouldZoom = true,
            zoomAmount = 4f,
            zoomDuration = 1f,
            executeAfterDialogue = true 
        };
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
            Debug.Log("Mission2_Pick: CheckCompletion - Esperando evento...");
        }
    }

    protected override void OnCleanup()
    {

        PlayerWeaponController.OnPlayerWeaponPicked -= OnPlayerPicked;
    }

    [ContextMenu("Force Complete")]
    public void ForceComplete()
    {
        Debug.Log("Mission2_Pick: FORCE COMPLETE");
        CompleteMission();
    }
}
