using UnityEngine;

public class BoulderProjectile : MovingProjectile
{
    [SerializeField] private float flipRate; // flips per second
    
    private SpriteRenderer _spriteRenderer;
    private float _flipTimer;
    private int _flipIndex;

    private void Awake()
    { 
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void OnEnable()
    {
        _flipTimer = 0f;
        _flipIndex = 3;
        ApplyFlipState(3);
    }
    protected override void Move()
    {
        base.Move();
        Flip();
    }
    private void Flip()
    {
        if (_spriteRenderer == null || flipRate <= 0f) return;

        _flipTimer += Time.deltaTime;
        float step = 1f / flipRate;

        if (_flipTimer >= step)
        {
            _flipTimer -= step;
            _flipIndex = (_flipIndex + 1) % 4;
            ApplyFlipState(_flipIndex);
        }
    }
    private void ApplyFlipState(int idx)
    {
        switch (idx)
        {
            case 0: // X
                _spriteRenderer.flipX = true;
                _spriteRenderer.flipY = false;
                break;
            case 1: // X & Y
                _spriteRenderer.flipX = true;
                _spriteRenderer.flipY = true;
                break;
            case 2: // Y
                _spriteRenderer.flipX = false;
                _spriteRenderer.flipY = true;
                break;
            default: // None
                _spriteRenderer.flipX = false;
                _spriteRenderer.flipY = false;
                break;
        }
    }
}
