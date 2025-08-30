using System.Collections;
using UnityEngine;

namespace Attack
{
    [CreateAssetMenu(fileName = "DashAttack", menuName = "Attacks/DashThenAttack")]
    public class DashThenAttack : MeleeAttack
    {
        public float distance;
        public float speed;
        [SerializeField] protected BaseAttack nextAttack;
        public override void ExecuteAttack()
        {
            CoroutineRunner.StartCoroutine(Attack());
        }

        protected override IEnumerator Attack()
        {
            float stuckTimer = 0;
            Weapon.SetActive(false);
            var user = Move as MonoBehaviour;
            Move.Dash(user.transform.up, speed, distance);
            Status.SetStatus(StatusEnum.Dashing, true);
            while (Status.GetStatus(StatusEnum.Dashing))
            {
                stuckTimer += Time.deltaTime;
                if (stuckTimer > 3)
                {
                    Status.SetStatus(StatusEnum.Dashing, false);
                }
                yield return 0;
            }
            nextAttack.SetUp(Weapon, Move, Look, Status, CoroutineRunner,Target);
            nextAttack.AttackFinish += SecondAttackFinish;
            nextAttack.ExecuteAttack();
        }

        private void SecondAttackFinish()
        {
            FinishAttack();
        }
    }
    
}