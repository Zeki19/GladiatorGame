using System.Collections;
using UnityEngine;

namespace Attack
{
    [CreateAssetMenu(fileName = "RangedAttack", menuName = "Attacks/ConeShotBarrage")]
    public class ConeShotBarrage : ConeShot
    {
        [SerializeField] private int numberOfRounds;
        [SerializeField] private float delayBetweenRounds;
        public override void ExecuteAttack()
        {
            CoroutineRunner.StartCoroutine(Attack());
        }
        protected override IEnumerator Attack()
        {
            for (int i = 0; i < numberOfRounds; i++)
            {
                Shoot();
                yield return new WaitForSeconds(delayBetweenRounds);
            }
            FinishAttack();
        }
    }
}