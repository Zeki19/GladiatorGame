using System;
using TMPro;
using UnityEngine;

public class HoundView : MonoBehaviour, ILook
{
    public float rotationSpeed;
    [SerializeField] private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        LookDir(_rb.linearVelocity);
    }

    public void LookDir(Vector2 dir)
    {
        if (dir == Vector2.zero) return;

        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

        float angle = Mathf.MoveTowardsAngle(
            _rb.rotation,
            targetAngle,
            rotationSpeed * Time.deltaTime
        );

        _rb.MoveRotation(angle);
    }
}
