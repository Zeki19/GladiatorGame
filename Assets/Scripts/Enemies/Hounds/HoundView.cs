using System;
using TMPro;
using UnityEngine;

public class HoundView : MonoBehaviour, ILook
{
    public float rotationSpeed;
    [SerializeField] private Rigidbody2D _rb;
    private Animator _animator;

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
    public void SetWalking(bool value)
    {
        _animator.SetBool("IsWalking", value);
    }

    public void PlayAttack()
    {
        _animator.SetTrigger("IsAttacking");
    }

    public void SetRunningAway(bool value)
    {
        _animator.SetBool("IsRunningAway", value);
    }

    public void PlayIdle()
    {
        _animator.SetTrigger("IsIdle");
    }
}
