using Interfaces;
using UnityEngine;

namespace Entities
{
    public class EntityManager : MonoBehaviour
    {
        private IHealth _healthSystem =new HealthSystem.HealthSystem(100);
        private IMove _move;
        private IAttack _attack;
        private ILook _look;
        [SerializeField]private Rigidbody2D rb;
        public Rigidbody2D Rb => rb;
    }
}
