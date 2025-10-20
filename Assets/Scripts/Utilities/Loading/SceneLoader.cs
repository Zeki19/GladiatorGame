using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadScene(string targetScene)
    {
        LoadingScreen.sceneToLoad = targetScene;
        SceneManager.LoadScene("LoadingScene");
    }
}
