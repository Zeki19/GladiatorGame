using Player;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Missions/Mission4_ChargedAttack", fileName = "Mission4_ChargedAttack")]
public class Mission4_ChargedAttack : TutorialMission
{
    private bool _hasCompleted = false;

    protected override void OnInitialize()
    {
        missionName = "Charged Attack";
        missionDescription = "Landing attacks charges your Charged attack, press Right Click to use it once the blue bar is full.";
        dialogueToPlay = EnumDialogues.Mission4;
        _hasCompleted = false;

        PlayerWeaponController.OnPlayerChargedAttack += OnChargedAttackUsed;
    }

    private void OnChargedAttackUsed()
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
            LogWaitingOnce("Mission4_ChargedAttack: CheckCompletion - Esperando uso de ataque cargado...");
        }
    }

    protected override void OnCleanup()
    {
        PlayerWeaponController.OnPlayerChargedAttack -= OnChargedAttackUsed;
    }

    [ContextMenu("Force Complete")]
    public void ForceComplete()
    {
        Debug.Log("Mission4_ChargedAttack: FORCE COMPLETE");
        CompleteMission();
    }
}