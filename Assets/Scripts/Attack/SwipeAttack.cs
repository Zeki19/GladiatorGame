using System.Collections;
using Attack;
using UnityEngine;

namespace Enemies.Attack
{
    [CreateAssetMenu(fileName = "SwipeAttack", menuName = "Attacks/SwipeAttack")]
    public class SwipeAttack : MeleeAttack
    {
        public AnimationCurve curve;
        public float startingAngle=-90;

        public override void ExecuteAttack()
        {
            Move.StopMovement();
            CoroutineRunner.StartCoroutine(Attack());
        }

        protected override IEnumerator Attack()
        {
            float animationClock = 0;
            float animationTime = 0;
            Weapon.SetActive(true);
            collider.enabled = true;
            if (curve.length > 0)
                animationTime = curve.keys[curve.length - 1].time;
            Weapon.transform.localPosition = Weapon.transform.localPosition.normalized *1.3f;
            while(animationClock < animationTime)
            {
                animationClock += Time.deltaTime;
                Weapon.transform.localRotation = Quaternion.Euler(0, 0, startingAngle+curve.Evaluate(animationClock));
                yield return 0;
            }
            collider.enabled = false;
            FinishAttack();
        }
    }
}