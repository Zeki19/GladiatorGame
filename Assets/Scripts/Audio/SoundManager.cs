using System;
using Player;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
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
        ServiceLocator.Instance.RegisterService(this);
    }

    private void Start()
    {
        ServiceLocator.Instance.GetService<PlayerManager>().Sounds += PlayerSfx;
        ServiceLocator.Instance.GetService<EnemyManager>().Sounds += EnemySfx;
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
                if (playerS != null)
                {
                    if(playerS.loop)
                    {
                        PlayMusicPlayer(playerS);
                        return;
                    }
                    PlaySoundPlayer(playerS);
                }
                break;
            }
            case "Enemy":
                Sound enemyS = Array.Find(EnemyPlaylist.sounds, sound => sound.name == a);
                if (enemyS != null)
                {
                    if (enemyS.loop)
                    {
                        PlayMusicEnemy(enemyS);
                        return;
                    }
                    PlaySoundEnemy(enemyS);
                }
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

    private void PlayerSfx(string sfxName)
    {
        Sound playerS = Array.Find(PlayerPlaylist.sounds, sound => sound.name == sfxName);
        if (playerS == null) return;
        
        Debug.Log("Player said: " + playerS.name);
        
        if(playerS.loop)
        {
            PlayMusicPlayer(playerS);
            return;
        }
        PlaySoundPlayer(playerS);
    }
    private void EnemySfx(string sfxName)
    {
        Sound enemyS = Array.Find(EnemyPlaylist.sounds, sound => sound.name == sfxName);
        if (enemyS == null) return;
        
        Debug.Log("Enemy said: " + enemyS.name);
        
        if (enemyS.loop)
        {
            PlayMusicEnemy(enemyS);
            return;
        }
        PlaySoundEnemy(enemyS);
    }
    
    public void PlayAudioClip(AudioClip sound)
    {
        playerSource.loop = false;
        enemySource.PlayOneShot(sound);
    }

}

