using System;
using Entities.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

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
        public int[] phasesThresholds;
        public PhaseSystem PhaseSystem;
        public IHealth HealthComponent => HealthSystem;
        
        public void PlaySound (string soundName,string entityName) => Sounds?.Invoke(soundName,entityName);

        protected virtual void Start()
        {
            PhaseSystem = new PhaseSystem(phasesThresholds, HealthComponent);
        }

        public event Action<string, string> Sounds;
        public Action<string> StopSounds;
    }
}
