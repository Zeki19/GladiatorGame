using Player;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Missions/Mission3_Attack", fileName = "Mission3_Attack")]
public class Mission3_Attack : TutorialMission
{
    private bool _hasCompleted = false;
    private DummyModel _dummyModel;

    protected override void OnInitialize()
    {
        missionName = "Basic Attack";
        missionDescription = "Press the left click to strike me with your blade";
        dialogueToPlay = EnumDialogues.Mission3;

        shouldMoveCamera = true;
        cameraEvent = new CameraEventConfig
        {
            eventId = "Dummy",
            targetTag = "Dummy",
            moveDuration = 1f,
            shouldZoom = true,
            zoomAmount = 3f,
            zoomDuration = 1f,
            executeAfterDialogue = true
        };
        _hasCompleted = false;

        var dummyManager = ServiceLocator.Instance.GetService<EnemyManager>();
        if (dummyManager != null)
        {
            _dummyModel = dummyManager.model as DummyModel;
            if (_dummyModel != null && dummyManager.HealthComponent != null)
            {
                dummyManager.HealthComponent.OnDamage += OnDummyTookDamage;
            }
            else
            {
                Debug.LogWarning("Mission3_Attack: No se pudo obtener DummyModel o HealthComponent");
            }
        }
        else
        {
            Debug.LogWarning("Mission3_Attack: No se pudo obtener EnemyManager del ServiceLocator");
        }
    }

    private void OnDummyTookDamage(float damage)
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
            Debug.Log("Mission3_Attack: CheckCompletion - Esperando que el dummy reciba daño...");
        }
    }

    protected override void OnCleanup()
    {
        var dummyManager = ServiceLocator.Instance.GetService<EnemyManager>();
        if (dummyManager != null && dummyManager.HealthComponent != null)
        {
            dummyManager.HealthComponent.OnDamage -= OnDummyTookDamage;
        }
    }

    [ContextMenu("Force Complete")]
    public void ForceComplete()
    {
        Debug.Log("Mission3_Attack: FORCE COMPLETE");
        CompleteMission();
    }
}