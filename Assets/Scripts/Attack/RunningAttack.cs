using System.Collections;
using UnityEngine;

namespace Attack
{
    [CreateAssetMenu(fileName = "RunningAttack", menuName = "Attacks/RunningAttack")]
    public class RunningAttack : MeleeAttack
    {
        public float distance;
        public float speed;

        public override void ExecuteAttack()
        {
            Move.StopMovement();
            CoroutineRunner.StartCoroutine(Attack());
        }

        protected override IEnumerator Attack()
        {
            Weapon.SetActive(true);
            collider.enabled = true;
            var user = Move as MonoBehaviour;
            Move.Dash(user.transform.up, speed,distance);
            Status.SetStatus(StatusEnum.Dashing, true);
            while (Status.GetStatus(StatusEnum.Dashing)) 
            {
                yield return 0;
            }
            collider.enabled = false;
            FinishAttack();
        }
    }
}