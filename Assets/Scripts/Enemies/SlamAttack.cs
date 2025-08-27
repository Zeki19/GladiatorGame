using System.Collections;
using Entities;
using Entities.Interfaces;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "SlamAttack", menuName = "Attacks/Slam")]
    public class SlamAttack : BaseAttack
    {
        [SerializeField] private GameObject areaPrefab;
        [SerializeField] private float delay;
        private GameObject _area;

        public override void SetUp(GameObject weapon, IMove move, ILook look, IStatus status,
            MonoBehaviour coroutineRunner)
        {
            base.SetUp(weapon, move, look, status, coroutineRunner);
            _area = Instantiate(areaPrefab, weapon.transform);
            _area.SetActive(false);
        }

        public override void ExecuteAttack()
        {
            Move.StopMovement();
            CoroutineRunner.StartCoroutine(Attack());
        }

        protected override IEnumerator Attack()
        {
            _area.transform.position=Weapon.transform.parent.position+Weapon.transform.parent.up.normalized*0.5f;
            yield return new WaitForSeconds(delay);
            _area.SetActive(true);
            yield return new WaitForSeconds(.5f);
            _area.SetActive(false);
            FinishAttack();
        }
    }
}