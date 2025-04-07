using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour, IRotatable
{
    public void Rotate(Vector2 direction)
    {
        if (direction == Vector2.zero) return;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 lookDir = (mouseWorldPos - transform.position);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
    }
}
