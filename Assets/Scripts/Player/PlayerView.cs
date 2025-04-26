using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerView : MonoBehaviour, ILook
{
    //M: Model
    //V: View
    //C: Controller
    [SerializeField]
    Animator _anim;
    Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void Update()
    {
        LookDir(Vector2.zero);
    }

    public void LookDir(Vector2 dir)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 lookDir = (mouseWorldPos - transform.position);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
    }

    public void LookSpeedMultiplier(float mult)
    {
        throw new System.NotImplementedException();
    }

    public void PlayStateAnimation(StateEnum state)
    {
        throw new System.NotImplementedException();
    }

    public void OnAttackAnim()
    {
        _anim.SetTrigger("Spin");
    }
    void OnMoveAnim()
    {
        _anim.SetFloat("Vel", _rb.linearVelocity.magnitude);
    }
}
