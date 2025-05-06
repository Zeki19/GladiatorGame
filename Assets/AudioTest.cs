using UnityEngine;

using UnityEngine;

public class AudioTest : MonoBehaviour
{
    [Header("Prueba de Audio")]
    public string sfxName;
    public string musicName;

    private AudioManager audioManager;

    void Start()
    {

        audioManager = ServiceLocator.Instance.GetService<AudioManager>();


        audioManager.PlayMusic(musicName);


        audioManager.PlaySFX(sfxName);
    }

    void Update()
    {
            audioManager.PlaySFX(sfxName);
        

        // Subir volumen de música con flecha arriba
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            audioManager.SetMusicVolume(1f);
        }

        // Bajar volumen de música con flecha abajo
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            audioManager.SetMusicVolume(0.2f);
        }
    }
}