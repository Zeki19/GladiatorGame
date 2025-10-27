using System;
using System.Collections;
using Player;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    [SerializeField] private CinematicManager cinematicManager;
    [SerializeField] private  EnemyManager enemy;
    [SerializeField] private  PlayerManager player;
    private void Start()
    {
        //StartCoroutine(WaitForFrames(5));
        
        cinematicManager.Initialize();
        
        enemy.HealthComponent.OnDead += VictoryCinematic;
        
        player.HealthComponent.OnDead += DefeatCinematic;
        
        IntroductionCinematic();
    }
    private void IntroductionCinematic()
    {
        cinematicManager.IntroCinematic();
    }
    private void VictoryCinematic()
    {
        cinematicManager.VictoryCinematic();
        Unsubscribe();
    }
    private void DefeatCinematic()
    {
        cinematicManager.DefeatCinematic();
        Unsubscribe();
    }
    private void Unsubscribe()
    {
        enemy.HealthComponent.OnDead += VictoryCinematic;
        player.HealthComponent.OnDead += DefeatCinematic;
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
