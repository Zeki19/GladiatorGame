using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "PokeAttack", menuName = "Weapons/PokeAttack")]
public class PokeAttack : EnemyBaseAttack
{
    public AnimationCurve curve;
    public float Delay;

    public override void ExecuteAttack()
    {
        Move.StopMovement();
        CoroutineRunner.StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        float animationClock = 0;
        float animationTime = 0;
        
        if (curve.length > 0)
            animationTime = curve.keys[curve.length - 1].time;
        Weapon.SetActive(true);
        Weapon.GetComponent<Collider2D>().enabled = false;
        Weapon.transform.localPosition = Weapon.transform.localPosition.normalized *curve.Evaluate(0.01f);
        yield return new WaitForSeconds(Delay);;
        Weapon.GetComponent<Collider2D>().enabled = true;
        while(animationClock < animationTime)
        {
            animationClock += Time.deltaTime;
            Weapon.transform.localPosition = Weapon.transform.localPosition.normalized *curve.Evaluate(animationClock);
            yield return 0;
        }
        FinishAttack();
    }
}