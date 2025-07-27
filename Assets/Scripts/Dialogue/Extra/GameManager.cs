using System.Collections;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DialogueManager dialogue;
    void Start()
    {
        StartCoroutine(DelayStart());
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(3f);

        dialogue.StartConversation(EnumDialogues.GaiusEntrance);
        dialogue.OnConversationEnd += ChangeScene;
    }

    void ChangeScene()
    {
        ServiceLocator.Instance.GetService<SceneChanger>().ChangeScene(3);
    }
}
