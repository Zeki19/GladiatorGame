using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si el jugador llegó
        if (collision.CompareTag("Player"))
        {
            Debug.Log("¡BOCA!");
            SceneManager.LoadScene("VictoryScene"); 
        }
    }
}
