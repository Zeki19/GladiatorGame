using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemy;

    public Vector2 GetEnemyPosition() 
    { 
        return enemy.transform.position;
    }

    public float GetEnemyHealthSystem()
    {
        return 0;
    }
}
