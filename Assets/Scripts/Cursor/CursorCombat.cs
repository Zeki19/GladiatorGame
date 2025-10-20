using System;
using Player;
using UnityEngine;

public class CursorCombat : MonoBehaviour
{
    [Header("Wiring")]
    [SerializeField] private PlayerWeaponController weapon;
    [SerializeField] private CursorController cursor;

    private void Start()
    {
        PauseManager.OnAnyPauseStateChanged += InGamePause;
        weapon.OnHasWeapon += WeaponCheck;
    }

    void InGamePause(bool state)
    {
        cursor.ChangeProfile(state ? "Main" : "Combat");
    }
    void WeaponCheck(bool state)
    {
        if (state)
        {
            cursor.ChangeProfile("Combat");
            weapon.Weapon.OnCooldown += CooldownCheck;
        }
        else
        {
            cursor.ChangeProfile("Main");
        }
    }
    void CooldownCheck(bool state)
    {
        cursor.SecondaryTexture(state);
    }
    
    private void OnDisable()
    {
        PauseManager.OnAnyPauseStateChanged -= InGamePause;
        weapon.OnHasWeapon -= WeaponCheck;
        weapon.Weapon.OnCooldown -= CooldownCheck;
    }
    private void OnDestroy()
    {
        PauseManager.OnAnyPauseStateChanged -= InGamePause;
        weapon.OnHasWeapon -= WeaponCheck;
        weapon.Weapon.OnCooldown -= CooldownCheck;
    }
}
