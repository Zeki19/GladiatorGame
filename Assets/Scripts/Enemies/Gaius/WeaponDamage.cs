using Attack;
using Entities;
using UnityEngine;

namespace Enemies.Gaius
{
    public class WeaponDamage : MonoBehaviour
    {
        private EnemyController _enemyController;
        private EnemyModel _enemyModel;
        private IStatus _status;
        private AttackManager _attack;
        private void Awake()
        {
            _enemyController = GetComponentInParent<EnemyController>();
            _status = _enemyController;
            _enemyModel = GetComponentInParent<EnemyModel>();
            _attack = GetComponentInParent<AttackManager>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) 
            {
                return;
            }
            _status.SetStatus(StatusEnum.AttackMissed,false);
            _enemyModel.AttackTarget(collision.transform, _attack.GetAttackDamage(_enemyController.currentAttack));
        }
    }
}
