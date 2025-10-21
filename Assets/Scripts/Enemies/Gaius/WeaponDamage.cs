using System;
using Attack;
using Entities;
using UnityEngine;

namespace Enemies.Gaius
{
    public class WeaponDamage : MonoBehaviour
    {
        protected EnemyController _enemyController;
        protected EnemyModel _enemyModel;
        protected IStatus _status;
        protected AttackManager _attack;
        [SerializeField] protected LayerMask _layerMask;
        private void Awake()
        {
            _enemyController = GetComponentInParent<EnemyController>();
            _status = _enemyController;
            _enemyModel = GetComponentInParent<EnemyModel>();
            _attack = GetComponentInParent<AttackManager>();
        }

        

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player")) 
            {
                _status.SetStatus(StatusEnum.AttackMissed,false);
                _enemyModel.AttackTarget(collision.transform, _attack.GetAttackDamage(_enemyController.currentAttack));
            }
        }
    }
}
