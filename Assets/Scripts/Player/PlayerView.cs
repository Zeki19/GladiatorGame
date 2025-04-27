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

    private int _oldSector = 1;

    private static readonly int TopRight = Animator.StringToHash("TopRight");
    private static readonly int Top = Animator.StringToHash("Top");
    private static readonly int TopLeft = Animator.StringToHash("TopLeft");
    private static readonly int BottomLeft = Animator.StringToHash("BottomLeft");
    private static readonly int Bottom = Animator.StringToHash("Bottom");
    private static readonly int BottomRight = Animator.StringToHash("BottomRight");
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
        RotateSprite(lookDir);
    }

    private void RotateSprite(Vector2 lookDir)
    {
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        if (angle < 0f)
        {
            angle += 360;
        }
        int sector = Mathf.FloorToInt((angle) / 60f) % 6;

        if (sector == _oldSector)
        {
            return;
        }
        _oldSector = sector;
        switch (sector)
        {
            case 0:
                _anim.SetTrigger(TopRight);
                break;
            case 1:
                _anim.SetTrigger(Top);
                break;
            case 2:
                _anim.SetTrigger(TopLeft);
                break;
            case 3:
                _anim.SetTrigger(BottomLeft);

                break;
            case 4:
                _anim.SetTrigger(Bottom);
                break;
            case 5:
                _anim.SetTrigger(BottomRight);
                break;
        }
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
