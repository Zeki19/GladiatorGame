using System;
using Player;
using UnityEngine;

public class CursorCombat : MonoBehaviour
{
    [Header("Wiring")]
    [SerializeField] private PlayerWeaponController weapon;
    [SerializeField] private CursorController cursor;
    
    private void Update()
    {
        if (PauseManager.IsPaused || PauseManager.IsPausedCinematic)
        {
            Debug.Log("IsPaused");
            cursor.ChangeProfile("Main");
            return;
        }
        
        if (weapon.HasWeapon)
        {
            Debug.Log("HasWeapon");
            cursor.ChangeProfile("Combat");
            cursor.SwapCursorTexture(!weapon.Weapon.CanAttack());
        }
        else
        {
            Debug.Log("Main");
            cursor.ChangeProfile("Main");
        }
    }
}
