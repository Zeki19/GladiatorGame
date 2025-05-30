using System.Collections;
using System.Collections.Generic;
using Entities;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerView : EntityView, ILook
{
    private int _oldSector = 1;

    //private static readonly int TopRight = Animator.StringToHash("TopRight");
    //private static readonly int Top = Animator.StringToHash("Top");
    //private static readonly int TopLeft = Animator.StringToHash("TopLeft");
    //private static readonly int BottomLeft = Animator.StringToHash("BottomLeft");
    //private static readonly int Bottom = Animator.StringToHash("Bottom");
    //private static readonly int BottomRight = Animator.StringToHash("BottomRight");
    private static readonly int Direction = Animator.StringToHash("MoveDirection");

    public override void LookDir(Vector2 dir)
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
        animator.SetFloat(Direction,sector);
        //switch (sector)
        //{
        //    case 0:
        //        animator.SetTrigger(TopRight);
        //        break;
        //    case 1:
        //        animator.SetTrigger(Top);
        //        break;
        //    case 2:
        //        animator.SetTrigger(TopLeft);
        //        break;
        //    case 3:
        //        animator.SetTrigger(BottomLeft);
        //        break;
        //    case 4:
        //        animator.SetTrigger(Bottom);
        //        break;
        //    case 5:
        //        animator.SetTrigger(BottomRight);
        //        break;
        //}
    }
    public void LookSpeedMultiplier(float mult)
    {
        throw new System.NotImplementedException();
    }

    public override void PlayStateAnimation(StateEnum state)
    {
        throw new System.NotImplementedException();
    }

    public void OnAttackAnim()
    {
        animator.SetTrigger("Spin");
    }
    void OnMoveAnim()
    {
        animator.SetFloat("Vel", manager.Rb.linearVelocity.magnitude);
    }
}
