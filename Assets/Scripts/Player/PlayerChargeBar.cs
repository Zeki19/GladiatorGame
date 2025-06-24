using Player;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChargeBar : MonoBehaviour
{
    [SerializeField] private Slider chargeSlider;
    private PlayerWeaponController _weaponController;

    private void Start()
    {
        _weaponController = ServiceLocator.Instance.GetService<PlayerWeaponController>();

        if (chargeSlider == null)
            chargeSlider = GetComponent<Slider>();

        chargeSlider.value = 0f;
    }

    private void Update()
    {
        if (_weaponController != null && chargeSlider != null)
        {
            if (_weaponController.Weapon!=null)
                chargeSlider.value = _weaponController.CheckWeaponChargePercent();
            else
                chargeSlider.value = 0;
        }
    }
}