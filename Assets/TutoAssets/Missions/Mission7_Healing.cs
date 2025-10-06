using UnityEngine;
using Entities;

[CreateAssetMenu(menuName = "Tutorial/Missions/Mission7_Healing", fileName = "Mission7_Healing")]
public class Mission7_Healing : TutorialMission
{

    private bool _hasCompleted = false;
    private bool _damageApplied = false;
    private bool _foodSystemEnabled = false;
    private EntityManager _playerManager;

    protected override void OnInitialize()
    {
        missionName = "Learn To Heal";
        missionDescription = "Get close to food to heal yourself";
        dialogueToPlay = EnumDialogues.Mission7;

        shouldMoveCamera = true;
        cameraEvent = new CameraEventConfig
        {
            eventId = "Food",
            targetTag = "Food",
            targetName = "FoodSpawn",
            moveDuration = 0.5f,
            shouldZoom = true,
            zoomAmount = 4f,
            zoomDuration = 1f,
            executeAfterDialogue = true
        };

        _hasCompleted = false;
        _damageApplied = false;
        _foodSystemEnabled = false;

        var playerModel = ServiceLocator.Instance.GetService<Player.PlayerModel>();
        if (playerModel != null)
        {
            _playerManager = playerModel.Manager;
        }

        Food.OnFoodConsumed += OnFoodConsumed;
    }

    public override void UpdateMission()
    {
        base.UpdateMission();

        if (!_foodSystemEnabled)
        {
            var foodManager = ServiceLocator.Instance.GetService<FoodManager>();
            if (foodManager != null)
            {
                foodManager.EnableFoodSystem();
                _foodSystemEnabled = true;
                Debug.Log("Mission8: Food system enabled");
            }
        }

        if (_foodSystemEnabled && !_damageApplied && _playerManager != null)
        {
            ApplyDamageToPlayer();
        }
    }

    private void ApplyDamageToPlayer()
    {
        if (_playerManager != null && _playerManager.HealthComponent != null)
        {
            float damageAmount = 20f;
            _playerManager.HealthComponent.TakeDamage(damageAmount);
            _damageApplied = true;
            Debug.Log($"Mission8: Applied {damageAmount} damage to player");
        }
    }

    private void OnFoodConsumed()
    {
        if (!_hasCompleted && _damageApplied)
        {
            Debug.Log("Mission7: Player consumed food and healed");
            _hasCompleted = true;
            CompleteMission();
        }
    }

    protected override void CheckCompletion()
    {
    }

    protected override void OnCleanup()
    {
        Food.OnFoodConsumed -= OnFoodConsumed;
    }

    [ContextMenu("Force Complete")]
    public new void ForceComplete()
    {
        Debug.Log("Mission7_Healing: FORCE COMPLETE");
        CompleteMission();
    }
}