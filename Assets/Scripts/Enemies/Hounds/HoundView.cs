using System;
using TMPro;
using UnityEngine;

public class HoundView : MonoBehaviour, ILook
{
    public float rotationSpeed;
    [SerializeField] private Rigidbody2D _rb;
    private Animator _animator;
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Run = Animator.StringToHash("Run");

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    
    public void LookDir(Vector2 dir)
    {
        if (dir == Vector2.zero) return;

        float angle = Vector2.SignedAngle(Vector2.up, dir);
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void PlayStateAnimation(StateEnum state)
    {
        switch (state)
        {
            case StateEnum.Idle:
                _animator.SetTrigger(Idle);
                break;
            case StateEnum.Attack:
                _animator.SetTrigger(Idle);
                break;
            case StateEnum.Chase:
                _animator.SetTrigger(Run);
                break;
            case StateEnum.Patrol:
                _animator.SetTrigger(Walk);
                break;
            case StateEnum.Runaway:
                _animator.SetTrigger(Walk);
                break;
            case StateEnum.Walk:
            case StateEnum.Default:
            case StateEnum.Dash:
            case StateEnum.GoZone:
            default:
                Debug.LogWarning("No animation mapped for state: " + state);
                break;
        }
    }
}
