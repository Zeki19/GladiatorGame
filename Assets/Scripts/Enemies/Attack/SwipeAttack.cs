using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SwipeAttack", menuName = "Weapons/MeleeAttack")]
public class SwipeAttack : EnemyBaseAttack
{
    public AnimationCurve curve;

    public override void ExecuteAttack()
    {
        Move.StopMovement();
        CoroutineRunner.StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        float animationClock = 0;
        float animationTime = 0;
        Weapon.SetActive(true);
        if (curve.length > 0)
            animationTime = curve.keys[curve.length - 1].time;
        Weapon.transform.localPosition = Weapon.transform.localPosition.normalized *1.3f;
        while(animationClock < animationTime)
        {
            animationClock += Time.deltaTime;
            Weapon.transform.localRotation = Quaternion.Euler(0, 0, -90+curve.Evaluate(animationClock));
            yield return 0;
        }
        FinishAttack();
    }
}