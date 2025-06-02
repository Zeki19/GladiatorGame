using Entities;
using UnityEngine;
using Weapons;

namespace Player
{
    public class PlayerManager : EntityManager
    {
        private Weapon _weapon;
        [SerializeField] private float speed;
        [SerializeField] private float maxHp;

        public Weapon weapon
        {
            get => _weapon;
            set => _weapon = value;
        }

        private void Awake()
        {
            ServiceLocator.Instance.RegisterService(this);
            HealthSystem = new HealthSystem.HealthSystem(maxHp);
        }

        public LayerMask EnemyLayer;


        private void OnTriggerEnter2D(Collider2D other)
        {
            //Debug.Log(other.gameObject.layer);
            //Debug.Log(EnemyLayer.value);
            //if(other.gameObject.layer==8)
            //{
            //    Debug.Log("Hit");
            //}
        }
    }
}