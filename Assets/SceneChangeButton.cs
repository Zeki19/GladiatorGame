using UnityEngine;

public class SceneChangeButton : MonoBehaviour
{
    public void ChangeScene(string sceneName) => SceneChanger.Instance.ChangeScene(sceneName);
    public void ChangeScene(int sceneId) => SceneChanger.Instance.ChangeScene(sceneId);
    public void Quit() => Application.Quit();
}
