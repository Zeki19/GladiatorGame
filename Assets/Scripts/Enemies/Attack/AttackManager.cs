using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Enemies.Attack
{
    public class AttackManager : MonoBehaviour
    {
        public List<BaseAttack> attacks = new List<BaseAttack>();
        public GameObject weapon;
        private EntityModel _move;
        private EntityView _look;
        private EnemyController _controller;

        private void Awake()
        {
            _move = GetComponent<EntityModel>();
            _look = GetComponent<EntityView>();
            _controller = GetComponent<EnemyController>();
        }

        private void Start()
        {
            foreach (var attack in attacks)
            {
                attack.SetUp(weapon, _move, _look, _controller, _move);
                attack.AttackFinish += FinishAttack;
            }
        }
        public void ExecuteAttack(int index)
        {
            if (index >= 0 && index < attacks.Count)
            {
                attacks[index].ExecuteAttack();
                _controller.SetStatus(StatusEnum.AttackMissed, true);
                _controller.SetStatus(StatusEnum.Attacking, true);
            }
            else
            {
                Debug.LogError("Enemy Attack Out Of Index");
            }
        }

        public float GetAttackDamage(int index)
        {
            return attacks[index].damage;
        }
        private void FinishAttack()
        {
            _controller.SetStatus(StatusEnum.Attacking, false);
        }
        
    }
}