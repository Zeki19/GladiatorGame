using System.Collections.Generic;
using Enemies;
using Entities;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public List<EnemyBaseAttack> attacks = new List<EnemyBaseAttack>();

    public GameObject weapon;
    public EntityModel move;
    public EntityView look;
    public EnemyController controller;
    
    public void ExecuteAttack(int index)
    {
        if (index >= 0 && index < attacks.Count)
        {
            attacks[index].ExecuteAttack();
            controller.SetStatus(StatusEnum.AttackMissed,true);
            controller.SetStatus(StatusEnum.Attacking,true);
        }
    }

    public float GetAttackDamage(int index)
    {
        return attacks[index].damage;
    }

    private void FinishAttack()
    {
        controller.SetStatus(StatusEnum.Attacking,false);
    }

    private void Start()
    {
        foreach (var attack in attacks)
        {
            attack.SetUp(weapon,move,look,controller,move);
            attack.AttackFinish += FinishAttack;
        }
    }
}