using System;
using Enemies;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

[Serializable, GeneratePropertyBag]
[Condition(name: "AttackMiss", story: "Did the Attack Miss?", category: "Conditions", id: "4eb4c7adbb2581fcd52e8f4219279458")]
public class AttackMissCondition : Condition
{
    private bool StetUP;
    [SerializeReference] public BlackboardVariable<EnemyController> Controller;
    public override bool IsTrue()
    {
        
        return Controller.Value.GetStatus(StatusEnum.AttackMissed);
    }

    public override void OnStart()
    {
        if (!StetUP)
        {
            GameObject.GetComponent<BehaviorGraphAgent>().GetVariable("Controller", out Controller);
            Debug.Log("a");
            StetUP = true;
        }
    }

    public override void OnEnd()
    {
    }
}
