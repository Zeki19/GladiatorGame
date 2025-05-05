using UnityEngine;

public class AIContext
{
    public Transform selfTransform;
    public Transform playerTransform;
    public float attackRange;
    public FSM<StateEnum> stateMachine;
}
