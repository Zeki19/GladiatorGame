using Player;
using UnityEngine;
using UnityEngine.UI;


public class PlayerChargeBar : MonoBehaviour
{
    [Header("UI Images")]
    [SerializeField] private Image chargeImage;  
    [SerializeField] private Image trailImage;   

    [Header("Trail Settings")]
    [SerializeField] private float trailSpeed = 2f;

    private PlayerWeaponController _weaponController;

    private void Start()
    {
        _weaponController = ServiceLocator.Instance.GetService<PlayerWeaponController>();

        chargeImage.fillAmount = 0f;
        trailImage.fillAmount = 0f;
    }

    private void Update()
    {
        if (_weaponController == null) return;

        float target = (_weaponController.Weapon != null)
            ? _weaponController.CheckWeaponChargePercent()
            : 0f;

        chargeImage.fillAmount = target;

        if (target >= 1f)
        {
            chargeImage.color = Color.yellow;
        }
        else
        {
            chargeImage.color = Color.white;
        }

        if (trailImage.fillAmount > target)
        {
            trailImage.fillAmount = Mathf.MoveTowards(
                trailImage.fillAmount,
                target,
                trailSpeed * Time.deltaTime
            );
        }
        else
        {
            trailImage.fillAmount = target;
        }
    }
}