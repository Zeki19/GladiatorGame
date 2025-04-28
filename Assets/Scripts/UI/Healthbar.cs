using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image _bar;

    public void UpdateHealthbar(float maxHealth, float currentHealth)
    {
        var a = currentHealth / maxHealth;
        Debug.Log(a);
        _bar.fillAmount = a;
    }
    
}
