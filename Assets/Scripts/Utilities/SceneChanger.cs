using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    private static SceneChanger _instance;
    public static SceneChanger Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("SceneChanger", typeof(SceneChanger)).GetComponent<SceneChanger>();
            }
            return _instance;
        }
        private set => _instance = value;
    }

    [Header("Loading Canvas Prefab")]
    [SerializeField] private GameObject loadingCanvasPrefab;

    GameObject canvasInstance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        ServiceLocator.Instance.RegisterService(this);
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    public void ChangeScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneRoutine(SceneUtility.GetScenePathByBuildIndex(sceneIndex)));
    }

    public void Quit()
    {
        Application.Quit();
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
            if (!loadingCanvasPrefab)
            {
                Debug.LogError("SceneChanger: No Loading Canvas Prefab assigned!");
                yield break;
            }
        if (!canvasInstance) 
        { 
            canvasInstance = Instantiate(loadingCanvasPrefab);
            DontDestroyOnLoad(canvasInstance);
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (asyncLoad.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.3f);
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        yield return null;
        canvasInstance.SetActive(false);
    }
}
