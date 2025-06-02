using Entities.StateMachine;
using UnityEngine;

namespace Entities
{
    public abstract class EntityController : MonoBehaviour
    {
        [SerializeField] protected EntityManager manager;
        protected FSM<StateEnum> Fsm;
        protected virtual void Update()
        {
            Fsm.OnExecute();
        }
        protected abstract void InitializeFsm();
    }
}
