using UnityEngine;
using UnityEngine.UI;

public class ButtonSfx : MonoBehaviour
{
    [SerializeField] private Sound clickSound;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (!clickSound.clip || !audioSource) return;
        
        GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        audioSource.PlayOneShot(clickSound.clip, clickSound.volume);
    }
}
