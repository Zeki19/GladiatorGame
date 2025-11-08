using System;
using Player;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    public SO_Sounds playlist;

    [Header("Scene Music Configuration")]
    [SerializeField] private SO_SceneMusicConfig sceneMusicConfig;

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
        PlayerManager playerManager = ServiceLocator.Instance.GetService<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.Sounds += PlayerSfx;
            playerManager.StopSounds += WhoToStop;
        }

        EnemyManager enemyManager = ServiceLocator.Instance.GetService<EnemyManager>();
        if (enemyManager != null)
        {
            enemyManager.Sounds += EnemySfx;
        }

        PlaySceneMusic();
    }

    private void PlaySceneMusic()
    {
        if (sceneMusicConfig == null)
        {
            Debug.LogError("SceneMusicConfig no está asignado en el SoundManager");
            return;
        }

        string sceneName = SceneManager.GetActiveScene().name;
        Sound s = sceneMusicConfig.GetMusicForScene(sceneName);

        if (s != null && s.clip != null)
        {
            musicSource.volume = s.volume;
            musicSource.loop = s.loop;
            musicSource.clip = s.clip;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"No se encontró música para la escena '{sceneName}' o el Sound no tiene AudioClip asignado");
        }
    }

    private void WhoSaidIt(string a, string who)
    {
        switch (who)
        {
            case "Player":
                {
                    Sound playerS = Array.Find(PlayerPlaylist.sounds, sound => sound.name == a);
                    if (playerS != null)
                    {
                        if (playerS.loop)
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

        //Debug.Log("Player said: " + playerS.name);

        if (playerS.loop)
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

        //Debug.Log("Enemy said: " + enemyS.name);

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