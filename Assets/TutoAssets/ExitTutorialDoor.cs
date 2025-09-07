using UnityEngine;

public class ExitTutorialDoor : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ServiceLocator.Instance.GetService<SceneChanger>().ChangeScene(1);
        }

        Debug.Log("Oh estoy entrando");
    }

}
