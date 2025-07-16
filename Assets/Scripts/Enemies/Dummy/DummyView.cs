using Entities;
using Unity.VisualScripting;
using UnityEngine;

namespace Dummy
{
    public class DummyView : EntityView, ILook
    {
        private static readonly int HeavyDamage = Animator.StringToHash("HeavyDamage");
        private static readonly int MediumDamage = Animator.StringToHash("MediumDamage");
        private static readonly int LightDamage = Animator.StringToHash("LightDamage");
        private static readonly int Idle = Animator.StringToHash("Idle");
        private void Start()
        {
            manager.HealthComponent.OnDamage += TakingDamageAnimation;
        }
        public override void LookDir(Vector2 dir)
        {
            throw new System.NotImplementedException();
        }

        private void TakingDamageAnimation(float damageTaken)
        {
            Debug.Log(damageTaken);
            if (damageTaken >= 30)
            {
                animator.ResetTrigger(Idle);
                animator.SetTrigger(HeavyDamage);
            }
            else if (damageTaken >= 15) 
            {
                animator.ResetTrigger(Idle);
                animator.SetTrigger(MediumDamage);
            }
            else
            {
                animator.ResetTrigger(Idle);
                animator.SetTrigger(LightDamage);
            }


        }

        private void GoBackToIdle()
        {
            animator.ResetTrigger(HeavyDamage);
            animator.ResetTrigger(MediumDamage);
            animator.ResetTrigger(LightDamage);
            animator.SetTrigger(Idle);
        }
        public override void PlayStateAnimation(StateEnum state)
        {
            throw new System.NotImplementedException();
        }

        public override void StopStateAnimation(StateEnum state)
        {
            throw new System.NotImplementedException();
        }
    }
}