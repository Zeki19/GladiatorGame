using System;
using Entities;
using UnityEngine;
using Utilities.Factory.Essentials;

public class Food : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float _healingAmount;
    private Vector2Int _spawnPosition;
    
    public bool IsActive => _isActive;
    private bool _isActive;

    public event Action<Vector2Int> OnPickUp;

    private void Start()
    {
        SetState(false);
    }

    public void Initialize(SOFood food, Vector2Int spawnPoint)
    {
        transform.position = (Vector2)spawnPoint;
        _spawnPosition = spawnPoint;
        _healingAmount = food.healingAmount;
        spriteRenderer.sprite = food.foodSprite;
        
        SetState(true);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        other.GetComponent<EntityManager>().HealthComponent.Heal(_healingAmount);
        
        OnPickUp?.Invoke(_spawnPosition);
        
        SetState(false);
    }

    private void SetState(bool state)
    {
        _isActive = state;
        gameObject.SetActive(state);
    }
}
