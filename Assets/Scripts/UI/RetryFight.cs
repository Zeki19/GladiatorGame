using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryFight : MonoBehaviour
{
    public void ContinueGame()
    {
        SaveData saveData = SaveManager.Instance.GetCurrentSaveData();

        if (saveData != null)
        {
            SceneManager.LoadScene(saveData.nextSceneToLoad);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
