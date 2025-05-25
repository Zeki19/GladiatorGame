using Entities.Interfaces;
using UnityEngine;

namespace Entities
{
    public abstract class EntityManager : MonoBehaviour
    {
        protected IHealth HealthSystem ;
        protected IMove Move;
        protected IAttack Attack;
        protected ILook Look;
        [SerializeField] private Rigidbody2D rb;
        public Rigidbody2D Rb => rb;
        public EntityModel model;
        public EntityView view;
        public EntityController controller;
        public IHealth HealthComponent => HealthSystem;
    }
}
