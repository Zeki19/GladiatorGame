using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenTimeline : MonoBehaviour
{
    [SerializeField] private ImageEffect uade;
    [SerializeField] private ImageEffect icon;
    [SerializeField] private ImageEffect title;
    
    private void Start()
    {
        uade.gameObject.SetActive(false);
        icon.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
        
        Wait(1, UadeStart);
    }
    
    private void UadeStart()
    {
        uade.gameObject.SetActive(true);
        uade.FadeOut(0);
        
        uade.FadeIn(2, UadeWait);
    }
    private void UadeWait()
    {
        Wait(1f, UadeEnd);
    }
    private void UadeEnd()
    {
        uade.FadeOut(1, Break);
    }

    private void Break()
    {
        Wait(1.5f, IconStart);
    }

    private void IconStart()
    {
        icon.gameObject.SetActive(true);
        icon.ZoomOut(0);
        
        icon.ZoomIn(2, IconWait);
    }
    private void IconWait()
    {
        Wait(.5f, TitleStart);
    }
    
    private void TitleStart()
    {
        title.gameObject.SetActive(true);
        title.FadeOut(0);
        
        title.FadeIn(2, TitleWait);
    }
    private void TitleWait()
    {
        Wait(1.5f, HideAll);
    }

    private void HideAll()
    {
        icon.FadeOut(1.5f);
        title.FadeOut(1.5f, ChangeScene);
    }

    private void ChangeScene()
    {
        SplashScreen.HasPlayedSplashScreen = true;
        gameObject.SetActive(false);
    }

    #region Utils

        private void Wait(float seconds, Action onComplete)
        {
            StartCoroutine(WaitCoroutine(seconds, onComplete));
        }
        private IEnumerator WaitCoroutine(float duration, Action onComplete)
        {
            yield return new WaitForSeconds(duration);
            onComplete?.Invoke();
        }

    #endregion
}
