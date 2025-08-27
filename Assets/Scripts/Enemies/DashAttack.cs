using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
{
    [CreateAssetMenu(fileName = "DashAttack", menuName = "Attacks/DashAttack")]
    public class DashAttack : BaseAttack
    {
        public AnimationCurve curve;
        [FormerlySerializedAs("Delay")] public float delay;

        public float distance;
        public float speed;
        
        private Collider2D _collider;

        public override void ExecuteAttack()
        {
            Move.StopMovement();
            CoroutineRunner.StartCoroutine(Attack());
            _collider=Weapon.GetComponent<Collider2D>();
        }

        protected override  IEnumerator Attack()
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
                    HandleStuck();
                    CoroutineRunner.StopAllCoroutines();
                }

                yield return 0;
            }

            float animationClock = 0;
            float animationTime = 0;

            if (curve.length > 0)
                animationTime = curve.keys[curve.length - 1].time;
            Weapon.SetActive(true);
            //_collider.enabled = false;
            Weapon.transform.localPosition = Weapon.transform.localPosition.normalized * curve.Evaluate(0.01f);
            //yield return new WaitForSeconds(delay);
            _collider.enabled = true;
            while (animationClock < animationTime)
            {
                animationClock += Time.deltaTime;
                Weapon.transform.localPosition =
                    Weapon.transform.localPosition.normalized * curve.Evaluate(animationClock);
                yield return 0;
            }
            FinishAttack();
        }

        private void HandleStuck()
        {
            Status.SetStatus(StatusEnum.Dashing, false);
            FinishAttack();
        }
    }
}