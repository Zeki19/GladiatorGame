using System.Collections.Generic;
using Enemies;
using Entities;
using UnityEngine;

namespace Attack
{
    public class AttackManager : MonoBehaviour
    {
        public List<BaseAttack> attacks = new List<BaseAttack>();
        public GameObject weapon;
        private EntityModel _move;
        private EntityView _look;
        private EntityController _controller;
        private EntityManager _Manager;
       // public Dictionary<BaseAttack,float> 

        private void Awake()
        {
            _move = GetComponent<EntityModel>();
            _look = GetComponent<EntityView>();
            _controller = GetComponent<EntityController>();
        }

        private void Start()
        {
            foreach (var attack in attacks)
            {
                attack.SetUp(weapon, _move, _look, _controller, _move,_controller);
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
            if (_controller.GetStatus(StatusEnum.AttackMissed))
            {
                var enemyController = _controller as EnemyController;
                if (enemyController != null) enemyController.MissAttack.AddMissAttack();
            }
        }
    }
}