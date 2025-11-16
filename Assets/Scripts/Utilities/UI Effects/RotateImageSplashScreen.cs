using System;
using UnityEngine;
using UnityEngine.UI;

public class RotateImageSplashScreen : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float interval = 1f;
    
    private int _index = 0;
    
    public void NextImage()
    {
        _index++;
        if (_index >= sprites.Length)  _index = 0;                

        image.sprite = sprites[_index];
    }
    
    private void OnEnable()
    {
        InvokeRepeating(nameof(NextImage), interval, interval);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
