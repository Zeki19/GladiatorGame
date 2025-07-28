using System;
using Entities;
using Entities.Interfaces;
using UnityEngine;

[Serializable]
public abstract class EnemyBaseAttack : ScriptableObject
{
    public string attackName;
    public float damage;
    protected GameObject Weapon;
    protected MonoBehaviour CoroutineRunner;
    protected IMove Move;
    protected ILook Look;
    protected IStatus Status;
    public event Action AttackFinish;

    
    public abstract void ExecuteAttack();

    public void SetUp(GameObject weapon,IMove move,ILook look,IStatus status,MonoBehaviour coroutineRunner)
    {
        Weapon = weapon;
        Move = move;
        Look = look;
        Status = status;
        CoroutineRunner = coroutineRunner;
    }

    protected void FinishAttack()
    {
        AttackFinish?.Invoke();
    }
    //private EnemyController controller;
    //private ISound sound;
}