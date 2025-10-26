using System;
using System.Collections;
using Player;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    private CinematicManager _cinematicManager;
    private EnemyManager _enemy;
    private PlayerManager _player;
    private void Start()
    {
        StartCoroutine(WaitForFrames(5));
        
        _cinematicManager = ServiceLocator.Instance.GetService<CinematicManager>();
        
        _enemy = ServiceLocator.Instance.GetService<EnemyManager>();
        _enemy.HealthComponent.OnDead += VictoryCinematic;
        
        _player = ServiceLocator.Instance.GetService<PlayerManager>();
        _player.HealthComponent.OnDead += DefeatCinematic;

        IntroductionCinematic();
    }
    private void IntroductionCinematic()
    {
        _cinematicManager.IntroCinematic();
    }
    private void VictoryCinematic()
    {
        _cinematicManager.VictoryCinematic();
        Unsubscribe();
    }
    private void DefeatCinematic()
    {
        _cinematicManager.DefeatCinematic();
        Unsubscribe();
    }
    private void Unsubscribe()
    {
        _enemy.HealthComponent.OnDead += VictoryCinematic;
        _player.HealthComponent.OnDead += DefeatCinematic;
    }
    
    #region Utils

    private IEnumerator WaitForFrames(int frameCount)
    {
        for (int i = 0; i < frameCount; i++)
        {
            yield return null;
        }
    }

    #endregion

}
