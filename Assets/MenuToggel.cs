using UnityEngine;

public class MenuToggel : MonoBehaviour
{
     public void Toggle()
     {
         gameObject.SetActive(!gameObject.activeSelf);
         Time.timeScale = gameObject.activeSelf ? 0 : 1;
     }
}
