using System.Collections.Generic;
using Entities.StateMachine;
using UnityEngine;

namespace Enemies
{
    public class AIContext
    {
        public GameObject selfGameObject;
        public GameObject playerGameObject;
        public EnemyController controller;
        public EnemyModel model;
        public List<float> attackRanges = new List<float>();
        public FSM<EnemyStates> stateMachine;
        public List<(Vector2, float)> Points=new List<(Vector2, float)>();
    }
}
