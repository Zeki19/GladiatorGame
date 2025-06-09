using System.Collections.Generic;
using Entities.StateMachine;
using UnityEngine;

namespace Enemies
{
    public class AIContext
    {
        //public Transform selfTransform;
        public GameObject selfGameObject;
        public GameObject playerGameObject;
        public EnemyController controller;
        public EnemyModel model;
        //public Transform playerTransform;
        public float attackRange;
        public FSM<StateEnum> stateMachine;
        public List<(Vector2, float)> Points=new List<(Vector2, float)>();
    }
}
