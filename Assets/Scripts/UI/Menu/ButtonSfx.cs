using UnityEngine;
using UnityEngine.UI;

public class ButtonSfx : MonoBehaviour
{
    [SerializeField] private Sound clickSound;
    private AudioSource _audioSource;

    private void Awake()
    {
        if (!clickSound.clip) return;
        
        _audioSource = GetComponent<AudioSource>();

        _audioSource.volume = clickSound.volume;

        GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        if (_audioSource != null && clickSound != null) _audioSource.PlayOneShot(clickSound.clip);
    }
}
