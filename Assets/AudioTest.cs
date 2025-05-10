using UnityEngine;


public class AudioTest : MonoBehaviour
{

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioManager.Instance.PlayMusic("clubbed");
        }
    }
}