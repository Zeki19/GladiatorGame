using System;
using Player;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
    private SoundManager _instance;
    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    public SO_Sounds playlist;
    [Header("Player")]
    [SerializeField] private AudioSource playerSource;
    public SO_Sounds PlayerPlaylist;
    [Header("Enemy")]
    [SerializeField] private AudioSource enemySource;
    public SO_Sounds EnemyPlaylist;
    
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(gameObject);
        
        ServiceLocator.Instance.RegisterService(this);
    }

    private void Start()
    {
        ServiceLocator.Instance.GetService<PlayerManager>().Sounds += WhoSaidIt;
        ServiceLocator.Instance.GetService<EnemyManager>().Sounds += WhoSaidIt;
        ServiceLocator.Instance.GetService<PlayerManager>().StopSounds += WhoToStop;

        if (playlist.sounds.Length <= 0) return;
        Sound s = Array.Find(playlist.sounds, sound => sound.name == "Combat");
        musicSource.volume = s.volume;
        musicSource.loop = s.loop;
        musicSource.clip = s.clip;
        musicSource.Play();
    }

    private void WhoSaidIt(string a,string who)
    {
        switch (who)
        {
            case "Player":
            {
                Sound playerS = Array.Find(PlayerPlaylist.sounds, sound => sound.name == a);
                if (playerS.loop)
                {
                    PlayMusicPlayer(playerS);
                    return;
                }
                PlaySoundPlayer(playerS);
                break;
            }
            case "Enemy":
                Sound enemyS = Array.Find(EnemyPlaylist.sounds, sound => sound.name == a);
                if (enemyS.loop)
                {
                    PlayMusicEnemy(enemyS);
                    return;
                }
                PlaySoundEnemy(enemyS);
                break;
        }
    }

    public void PlayMusic(Sound sound)
    {
        musicSource.volume = sound.volume;
        musicSource.loop = sound.loop;
        musicSource.clip = sound.clip;
        musicSource.Play();
    }

    private void PlaySoundPlayer(Sound sound)
    {
        playerSource.volume = sound.volume;
        playerSource.loop = sound.loop;
        playerSource.PlayOneShot(sound.clip);
    }

    private void PlayMusicPlayer(Sound sound)
    {
        playerSource.volume = sound.volume;
        playerSource.loop = sound.loop;
        
        playerSource.clip = sound.clip;
        playerSource.Play();
    }

    private void WhoToStop(string who)
    {
        switch (who)
        {
            case "Player":
            {
                playerSource.Stop();
                break;
            }
            case "Enemy":
                enemySource.Stop();
                break;
        }
    }
    
    private void PlaySoundEnemy(Sound sound)
    {
        enemySource.volume = sound.volume;
        enemySource.loop = sound.loop;
        enemySource.PlayOneShot(sound.clip);
    }
    
    private void PlayMusicEnemy(Sound sound)
    {
        enemySource.volume = sound.volume;
        enemySource.loop = sound.loop;
        
        enemySource.clip = sound.clip;
        enemySource.Play();
    }
    
}

