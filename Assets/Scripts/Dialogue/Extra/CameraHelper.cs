using UnityEngine;

public class CameraHelper : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Transform _target;

    private void Update()
    {
        if (_target == null) return;
        
        transform.position = Vector3.Lerp(
            transform.position,
            _target.position,
            moveSpeed * Time.deltaTime
        );
    }

    public void MoveToTarget(Transform target)
    {
        _target = target;
    }
}
