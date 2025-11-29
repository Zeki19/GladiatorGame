using System;
using UnityEngine;

using System.Runtime.InteropServices;

public class MobileInput : MonoBehaviour
{
    [SerializeField] private GameObject ControlsHolder;
    public Joystick moveJoy;   // Left joystick
    public Joystick aimJoy;    // Right joystick
    
    public bool attackA;
    public bool attackB;

    private void Start()
    {
        if(IsMobile())
            ControlsHolder.SetActive(true);
    }

    void Update()
    {
        // Movement
        Move = new Vector2(moveJoy.Horizontal, moveJoy.Vertical);

        // Aim
        Aim = new Vector2(aimJoy.Horizontal, aimJoy.Vertical);
    }

    public Vector2 Move { get; private set; }
    public Vector2 Aim { get; private set; }

    // Called by UI Buttons
    public void AttackA() => attackA = true;
    public void AttackB() => attackB = true;

    public void ResetAttacks()
    {
        attackA = false;
        attackB = false;
    }
    [DllImport("__Internal")]
    private static extern int IsMobileBrowser();

    public bool IsMobile()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return IsMobileBrowser() == 1;
#else
        return false;
#endif
    }
}
