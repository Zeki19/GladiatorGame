using UnityEngine;
using UnityEngine.UI;

public class TitleConfiguration : MonoBehaviour
{
    [Header("Title wiring")]
    [SerializeField] private Image titleImage;

    [Header("Animation wiring")] 
    [SerializeField] private Image leftImage;
    [SerializeField] private Image rightImage;
    
    [Header("Animation config")]
    [Tooltip("Small: 32 || Medium: 64 || Large: 128")] [SerializeField] private Size imageSize;
    [SerializeField] private float frameRate = 10f;
    [SerializeField] private Sprite[] sprites;
    
    private int _currentFrame;
    private float _timer;

    private void Start()
    {
        frameRate = Mathf.Max(0.01f, frameRate);
        
        if (sprites == null || sprites.Length == 0)
        {
            OnlyTitle();
            return;
        }
        
        TitleAnimations();
    }
    private void Update()
    {
        if (sprites == null || sprites.Length == 0) return;
        if (!isActiveAndEnabled) return;

        _timer += Time.deltaTime;
        float frameDuration = 1f / frameRate;

        if (_timer >= frameDuration)
        {
            _timer -= frameDuration;
            _currentFrame = (_currentFrame + 1) % sprites.Length;

            if (leftImage)  leftImage.sprite  = sprites[_currentFrame];
            if (rightImage) rightImage.sprite = sprites[_currentFrame];
        }
    }
    private void OnlyTitle()
    {
        if (leftImage)  leftImage.gameObject.SetActive(false);
        if (rightImage) rightImage.gameObject.SetActive(false);
    }
    private void TitleAnimations()
    {
        var imageDimensions = (int)imageSize;
        Debug.Log(imageDimensions);
        if (leftImage)  leftImage.gameObject.SetActive(true);
        if (rightImage) rightImage.gameObject.SetActive(true);
        
        if (leftImage)  leftImage.rectTransform.sizeDelta  = new Vector2(imageDimensions, imageDimensions);
        if (rightImage) rightImage.rectTransform.sizeDelta = new Vector2(imageDimensions, imageDimensions);
        
        if (titleImage)
        {
            var rt = titleImage.rectTransform;
            rt.offsetMin = new Vector2(imageDimensions, rt.offsetMin.y);//Left
            rt.offsetMax = new Vector2(-imageDimensions, rt.offsetMax.y);//Right
        }
        
        if (sprites is { Length: > 0 })
        {
            if (leftImage)  leftImage.sprite  = sprites[0];
            if (rightImage) rightImage.sprite = sprites[0];
        }
    }
}

enum Size
{
    Small = 32,
    Medium = 64,
    Large = 128,
}
