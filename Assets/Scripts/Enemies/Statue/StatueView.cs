using UnityEngine;
using UnityEngine.Rendering;

public class StatueView : MonoBehaviour, ILook
{
    [SerializeField] float rotationSpeed;
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

    public void LookSpeedMultiplier(float mult)
    {
        throw new System.NotImplementedException();
    }

    public void PlayStateAnimation(StateEnum state)
    {
        throw new System.NotImplementedException();
    }
}
