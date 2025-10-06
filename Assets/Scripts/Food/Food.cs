using System;
using Entities;
using UnityEngine;
using Utilities.Factory.Essentials;

public class Food : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D foodCollider;

    private float _healingAmount;
    private Vector2 _spawnPosition;
    private bool _collisionEnabled = true;

    public bool IsActive => _isActive;
    private bool _isActive;

    public event Action<Vector2> OnPickUp;

    public static event Action OnFoodConsumed;

    private void Awake()
    {
        if (foodCollider == null)
        {
            foodCollider = GetComponent<Collider2D>();
        }
    }

    private void Start()
    {
        SetActive(false);
    }

    public void Initialize(SOFood food, Vector2 spawnPoint)
    {
        transform.position = spawnPoint;
        _spawnPosition = spawnPoint;
        _healingAmount = food.healingAmount;
        spriteRenderer.sprite = food.foodSprite;

        SetActive(true);
    }
    public void SetCollisionEnabled(bool enabled)
    {
        _collisionEnabled = enabled;
        if (foodCollider != null)
        {
            foodCollider.enabled = enabled;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_collisionEnabled || !other.CompareTag("Player")) return;

        var entityManager = other.GetComponent<EntityManager>();
        if (entityManager != null && entityManager.HealthComponent != null)
        {
            entityManager.HealthComponent.Heal(_healingAmount);

            OnFoodConsumed?.Invoke();

            OnPickUp?.Invoke(_spawnPosition);
            SetActive(false);
        }
    }

    private void SetActive(bool state)
    {
        _isActive = state;
        gameObject.SetActive(state);
    }
}
