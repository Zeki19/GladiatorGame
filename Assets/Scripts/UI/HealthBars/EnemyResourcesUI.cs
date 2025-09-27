using Entities;
using UnityEngine;

public class EnemyResourcesUI : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private EntityManager entityManager;
    
    [Header("Bars prefab")]
    [SerializeField] private GameObject barPrefab;
    [SerializeField] private GameObject barHolder;
    private ResourceBarTracker currentBar;

    private int index;
    private ResourceBarTracker[] rb;
    void Start()
    {
        entityManager.HealthComponent.OnDamage += Damage;
        entityManager.HealthComponent.OnHeal += Heal;
        entityManager.HealthComponent.OnDead += Dead;

        SetUpHealthBars();
    }
    
    private void SetUpHealthBars()
    {
        var array = entityManager.PhaseSystem.Phases();
        
        rb = new ResourceBarTracker[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            Debug.Log(array[i]);
            var bar = Instantiate(barPrefab, barHolder.transform, true);
            var script = bar.GetComponent<ResourceBarTracker>();
            
            script.SetUp(array[i], true, Random.ColorHSV());
            
            rb[i] = script;
        }
    }

    private void Damage(float value)
    {
        rb[index].ChangeResourceByAmount((int)value * -1);
    }
    private void Heal(float value)
    {
        rb[index].ChangeResourceByAmount((int)value);
    }
    private void Dead()
    {
        entityManager.HealthComponent.OnDamage -= Damage;
        entityManager.HealthComponent.OnHeal -= Heal;
        entityManager.HealthComponent.OnDead -= Dead;
    }
}
