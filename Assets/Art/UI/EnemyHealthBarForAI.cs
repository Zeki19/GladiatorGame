using UnityEngine;
using UnityEngine.UI;     
// using TMPro;      
// descomenta si usás TextMeshProUGUI

public class EnemyHealthBarForAI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image healthImage;   
    [SerializeField] private Image trailImage;    
    //[SerializeField] private Text nameText;      
    // [SerializeField] private TextMeshProUGUI nameText; 

    [Header("Trail Settings")]
    [SerializeField] private float trailSpeed = 2f;

    [Header("Enemy Settings")]
    public EnemyManager _manager;                 

    private void Start()
    {
        _manager.HealthComponent.OnDamage += OnHealthChanged;
        _manager.HealthComponent.OnHeal += OnHealthChanged;

        healthImage.fillAmount = 1f;
        trailImage.fillAmount = 1f;

        //nameText.text = _manager.BossName; 
        // nameText.text = _manager.name;  
    }

    private void OnHealthChanged(float _)
    {
        float target = _manager.HealthComponent.GetCurrentHealthPercentage() / 100f;
        healthImage.fillAmount = target;
        StopAllCoroutines();
        StartCoroutine(UpdateTrail(target));
    }

    private System.Collections.IEnumerator UpdateTrail(float target)
    {
        while (trailImage.fillAmount > target)
        {
            trailImage.fillAmount = Mathf.MoveTowards(
                trailImage.fillAmount,
                target,
                trailSpeed * Time.deltaTime
            );
            yield return null;
        }
        trailImage.fillAmount = target;
    }

    private void OnDestroy()
    {
        if (_manager != null && _manager.HealthComponent != null)
        {
            _manager.HealthComponent.OnDamage -= OnHealthChanged;
            _manager.HealthComponent.OnHeal -= OnHealthChanged;
        }
    }
}