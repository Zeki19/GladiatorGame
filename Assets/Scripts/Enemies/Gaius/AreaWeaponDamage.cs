using UnityEngine;

namespace Enemies.Gaius
{
    public class AreaWeaponDamage : WeaponDamage
    {
        private void OnEnable()
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, transform.localScale.x, _layerMask);
            if (hit != null)
            {
                _status.SetStatus(StatusEnum.AttackMissed,false);
                _enemyModel.AttackTarget(hit.transform, _attack.GetAttackDamage(_enemyController.currentAttack));
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            
        }
    }
}