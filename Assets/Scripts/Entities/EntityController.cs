using Entities.StateMachine;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Entities
{
    public abstract class EntityController : MonoBehaviour
    {
        public  Stats a;
        [SerializeField] protected EntityManager manager;
        protected FSM<StateEnum> Fsm;
        protected virtual void Update()
        {
            Fsm.OnExecute();
        }
        protected abstract void InitializeFsm();
    }
}
