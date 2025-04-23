using UnityEngine;

public class HoundView : MonoBehaviour, ILook
{
    public float rotationSpeed;
    public void LookDir(Vector2 dir)
    {
        if (dir == Vector2.zero) return;

        // Calculate target angle
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Current rotation as quaternion
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

        // Smoothly rotate towards target
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
