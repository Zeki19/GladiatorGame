using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Utilitys.Factory.WeaponFactory;

public class WeaponTest : MonoBehaviour
{
    [SerializeField] private SoWeapon testWeapon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [ContextMenu("CreateWeapon")]
    private void CreateWeapon()
    {
        ServiceLocator.Instance.GetService<WeaponManager>().CreateWeapon(testWeapon);
    }

    public void corrutineUser(IEnumerator corrutin)
    {
        StartCoroutine(corrutin);
    }
}
