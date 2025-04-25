using UnityEngine;

public class UISceneManager : MonoBehaviour
{
    public void ChangeScene(int sceneNumber)
    {
        ServiceLocator.Instance.GetService<SceneChanger>().ChangeScene(sceneNumber);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        return;
#endif 
        Application.Quit();
    }
}
