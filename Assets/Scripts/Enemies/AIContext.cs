using UnityEngine;

public class AIContext
{
    //public Transform selfTransform;
    public GameObject selfGameObject;
    public GameObject playerGameObject;
    //public Transform playerTransform;
    public float attackRange;
    public FSM<StateEnum> stateMachine;
}
