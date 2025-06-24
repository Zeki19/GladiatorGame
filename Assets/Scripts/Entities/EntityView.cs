using System;
using System.Collections;
using Entities;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class EntityView : MonoBehaviour, ILook
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected EntityManager manager;
    [SerializeField] protected SpriteRenderer sprite;
    private Coroutine _damageBlink;

    public abstract void LookDir(Vector2 dir);
    public abstract void PlayStateAnimation(StateEnum state);

    private void Start()
    {
        manager.HealthComponent.OnDamage += DamageBLinkActivator;
    }

    private void DamageBLinkActivator(float damage)
    {
        if (_damageBlink != null)
            StopCoroutine(_damageBlink);
        _damageBlink = StartCoroutine(DamagedBlink());
        
    }
    private IEnumerator DamagedBlink()
    {
        for (int i = 0; i < 3; i++)
        {
            sprite.color=Color.red;
            yield return new WaitForSeconds(.1f);
            sprite.color=Color.white;
            yield return new WaitForSeconds(.1f);
        }
    }
}
