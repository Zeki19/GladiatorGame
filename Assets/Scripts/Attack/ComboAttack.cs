using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Entities;
using Entities.Interfaces;
using UnityEngine;

namespace Attack
{
    [CreateAssetMenu(fileName = "ComboAttack", menuName = "Attacks/ComboAttack")]
    public class ComboAttack : BaseAttack
    {
        [SerializeField] private List<ComboAttackProperties> attacks;
        public bool continueToNextAttack;
        public override void SetUp(GameObject weapon, IMove move, ILook look, IStatus status, MonoBehaviour coroutineRunner, ITarget target)
        {
            base.SetUp(weapon, move, look, status, coroutineRunner, target);
            foreach (var attack in attacks)
            {
                attack.Attack.SetUp(weapon, move, look, status, coroutineRunner, target);
                attack.Attack.AttackFinish += ContinueToNextAttack;
            }
        }

        public override void ExecuteAttack()
        {
            continueToNextAttack = false;
            CoroutineRunner.StartCoroutine(Attack());
        }
        
        // Executes each attack in order, waiting for each to finish before proceeding.
        // The combo ends early depending on whether an attack hits or misses, based on its settings.
        protected override IEnumerator Attack()
        {
            foreach (var attack in attacks)
            {
                // For a combo to count as successful, the final attack must hit.
                Status.SetStatus(StatusEnum.AttackMissed,true);
                continueToNextAttack = false;
                yield return new WaitForSeconds(attack.Delay);
                attack.Attack.ExecuteAttack();
                yield return new WaitUntil(()=>continueToNextAttack);
                if(attack.BreakComboIfHit&&!Status.GetStatus(StatusEnum.AttackMissed))
                    break;
                if(attack.BreakComboIfMiss&Status.GetStatus(StatusEnum.AttackMissed))
                    break;
            }
            FinishAttack();
        }

        private void ContinueToNextAttack()
        {
            continueToNextAttack = true;
        }
        
        [Serializable]
        struct ComboAttackProperties
        {
            public BaseAttack Attack;
            public float Delay;
            public bool BreakComboIfMiss;
            public bool BreakComboIfHit;
        }
    }
}