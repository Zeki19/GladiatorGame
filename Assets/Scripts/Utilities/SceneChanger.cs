using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private static SceneChanger _instance;

    public static SceneChanger Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("service Locator", typeof(SceneChanger)).GetComponent<SceneChanger>();
            }

            return _instance;
        }
        private set => _instance = value;
    }

    private void Awake()
    {
        ServiceLocator.Instance.RegisterService(this);
    }
    

    public void ChangeScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit() 
    {
        Application.Quit();
    }
}
