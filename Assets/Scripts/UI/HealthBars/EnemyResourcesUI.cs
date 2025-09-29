using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entities;
using UnityEngine;

public class EnemyResourcesUI : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private EntityManager entityManager;
    
    [Header("Bars")]
    [SerializeField] private GameObject barPrefab;
    [SerializeField] private GameObject barHolder;
    [SerializeField] private ResourceBarTracker.DisplayType displayType;
    
    [Header("Image")]
    [SerializeField] private UnityEngine.UI.Image headImage;
    
    [Header("This should not be here but in the Enemy...")]
    public Sprite[] sprites;

    private int _index;
    private ResourceBarTracker[] _rb;
    private float[] _health;
    private float _actualHealth;
    
    private void Start()
    {
        entityManager.HealthComponent.OnDamage += Damage;
        entityManager.HealthComponent.OnHeal += Heal;
        entityManager.HealthComponent.OnDead += Dead;

        StartCoroutine(Wait(.25f));
    }
    private IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SetUpHealthBars();
        SetImage();
        _rb[_index].SetTextField(displayType);
    }
    private void SetUpHealthBars()
    {
        _health = entityManager.PhaseSystem.PhasesHealth();
        
        _rb = new ResourceBarTracker[_health.Length];
        
        var color = GenerateColors(_health.Length);

        for (int i = 0; i < _health.Length; i++)
        {
            var bar = Instantiate(barPrefab, barHolder.transform, false);
            var script = bar.GetComponent<ResourceBarTracker>();
            
            script.SetUp(_health[i], true, color[i]);
            script.SetTextField(ResourceBarTracker.DisplayType.None);
            
            _rb[i] = script;
        }

        _index = _health.Length - 1;
        _actualHealth = _health[_index];
    }
    private Color[] GenerateColors(int steps)
    {
        var colors = new Color[steps];
        
        Color red = Color.red;
        Color orange = new Color(1f, 0.5f, 0f);

        for (int i = 0; i < steps; i++)
        {
            var t = i / (float)(steps - 1);
            var c = Color.Lerp(orange, red, t);
            colors[i] = c;
        }

        return colors;
    }

    private void SetImage()
    {
        if (sprites[_index])
        {
            headImage.sprite = sprites[_index];
        }
    }
    
    private void Damage(float value)
    {
        _actualHealth -= value;
        _rb[_index].ChangeResourceByAmount(value * -1f);

        if (_actualHealth > 0) return;
        
        _rb[_index].SetTextField(ResourceBarTracker.DisplayType.None);
        
        _index--;
        if (_index < 0) _index = 0;
        
        _rb[_index].ChangeResourceByAmount(_actualHealth);
        _actualHealth = _health[_index];
        
        _rb[_index].SetTextField(displayType);
        SetImage();
    }
    private void Heal(float value) //Might not work as intended.
    {
        _actualHealth += value;
        _rb[_index].ChangeResourceByAmount(value);
        
        if (_actualHealth > _health[_index]) return;
        
        _index++;
        if (_index > _health[_index]) _index = _health.Length;
        
        _rb[_index].ChangeResourceByAmount(_actualHealth);
        _actualHealth = _health[_index];
        
        SetImage();
    }
    private void Dead()
    {
        entityManager.HealthComponent.OnDamage -= Damage;
        entityManager.HealthComponent.OnHeal -= Heal;
        entityManager.HealthComponent.OnDead -= Dead;
    }

}
