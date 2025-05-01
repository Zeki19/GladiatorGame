using System;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    [SerializeField] protected EntityManager manager;
    [SerializeField] protected FSM<StateEnum> Fsm;
    protected virtual void Update()
    {
        Fsm.OnExecute();
    }
    protected abstract void InitializeFsm();
}
