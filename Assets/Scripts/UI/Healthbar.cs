using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image _bar;

    public void UpdateHealthbar(float maxHealth, float currentHealth)
    {
        _bar.fillAmount = currentHealth / maxHealth;
    }
    
}
