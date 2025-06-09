using System;
using System.Collections;
using Entities;
using UnityEngine;
using UnityEngine.Serialization;

public class PowerUp : MonoBehaviour
{
    [SerializeField]private PowerUpType type;
    [SerializeField] private float value;
    [SerializeField] private float duration;
    [SerializeField] private float reappear;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D collider2D;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private int weight;
    [SerializeField] private int range;
    private EntityManager _collisionManager;
    private static readonly int BuffType = Animator.StringToHash("BuffType");
    private static readonly int Use = Animator.StringToHash("Use");
    private static readonly int Reset = Animator.StringToHash("Reset");

    private void Start()
    {
        ServiceLocator.Instance.GetService<GridManager>().AddPickUp(transform.position,weight,range, type);
        switch (type)
        {
            case PowerUpType.Heal:
                animator.SetFloat(BuffType,0);
                break;
            case PowerUpType.Speed:
                animator.SetFloat(BuffType,1);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out _collisionManager))
        {
            switch (type)
            {
                case PowerUpType.Heal:
                    _collisionManager.HealthComponent.Heal(value);
                    animator.SetFloat(BuffType,0);
                    break;
                case PowerUpType.Speed:
                    _collisionManager.model.ModifySpeed(value);
                    animator.SetFloat(BuffType,1);
                    break;
            }

            StartCoroutine(CountDown());
            animator.SetTrigger(Use);
        }
    }

    public void TurnOff()
    {
        collider2D.enabled = false;
        renderer.enabled = false;
    }

    private IEnumerator CountDown()
    {
        animator.ResetTrigger(Reset);
        yield return new WaitForSeconds(duration);
        switch (type)
        {
            case PowerUpType.Speed:
                _collisionManager.model.ModifySpeed(-value);
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(reappear);
        collider2D.enabled = true;
        renderer.enabled = true;
        animator.SetTrigger(Reset);
        animator.ResetTrigger(Use);
    }
}
public enum PowerUpType
{
        Heal,
        Speed
}