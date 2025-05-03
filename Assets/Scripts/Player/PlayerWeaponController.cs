using System;
using UnityEngine;
using Weapons;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField]private GameObject playerRotation;
    [SerializeField] private PlayerManager _manager;
    public float offset;
    private Weapon _weapon;
    

    void Start()
    {
        CreateAWeapon();
    }

    void Update()
    {
        lookDir();
    }

    private void lookDir()
    {
        transform.rotation = playerRotation.transform.rotation;
        transform.position = playerRotation.transform.position +(playerRotation.transform.up)*offset;
    }
    public void CreateAWeapon()
    {
        _weapon=ServiceLocator.Instance.GetService<WeaponManager>().GetWeapon();
        _weapon.WeaponGameObject.transform.parent = this.transform;
        _manager.weapon = _weapon;
    }

    private void OnTriggerEnter(Collider other)
    {
        throw new NotImplementedException();
    }
}
