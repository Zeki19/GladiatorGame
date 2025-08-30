using System.Collections;
using Attack;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PokeAttack", menuName = "Attacks/PokeAttack")]
public class PokeAttack : MeleeAttack
{
    public AnimationCurve curve;
    [FormerlySerializedAs("Delay")] public float delay;
    private Collider2D _collider;

    public override void ExecuteAttack()
    {
        _collider=Weapon.GetComponent<Collider2D>();
        Move.StopMovement();
        CoroutineRunner.StartCoroutine(Attack());
    }

    protected override IEnumerator Attack()
    {
        float animationClock = 0;
        float animationTime = 0;
        
        if (curve.length > 0)
            animationTime = curve.keys[curve.length - 1].time;
        Weapon.SetActive(true);
        collider.enabled = true;
        _collider.enabled = false;
        Weapon.transform.localPosition = Weapon.transform.localPosition.normalized *curve.Evaluate(0.01f);
        yield return new WaitForSeconds(delay);;
        _collider.enabled = true;
        while(animationClock < animationTime)
        {
            animationClock += Time.deltaTime;
            Weapon.transform.localPosition = Weapon.transform.localPosition.normalized *curve.Evaluate(animationClock);
            yield return 0;
        }
        collider.enabled = false;
        FinishAttack();
    }
}