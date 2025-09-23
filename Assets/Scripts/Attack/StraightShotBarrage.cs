using System.Collections;
using UnityEngine;

namespace Attack
{
    [CreateAssetMenu(fileName = "RangedAttack", menuName = "Attacks/StraightShotBarrage")]
    public class StraightShotBarrage : StraightShot
    {
        [SerializeField] private int numberOfRounds;
        [SerializeField] private float delayBetweenRounds;
        [SerializeField] private bool repositionAfterEachShot;
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
                if(repositionAfterEachShot)
                    LookPosition(Target.GetTarget().transform.position);
            }
            FinishAttack();
        }
    }
}