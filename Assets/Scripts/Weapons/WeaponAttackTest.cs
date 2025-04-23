using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Weapons;

public class WeaponAttackTest : MonoBehaviour
{
    public Weapon weapon;
    [SerializeField] private PlayerInput playerInput;
    private InputAction _attackAction;
    public bool _test;
    [SerializeField]private WeaponManager _manager;
    void Start()
    {
        var actionMap = playerInput.actions.FindActionMap("Player");
        _attackAction = actionMap.FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        if (_attackAction.triggered)
        {
            _test = true;
            weapon = _manager.GetWeapon();
        }

        if (_test)
        {
            weapon.basicAttack();
        }
    }
}
