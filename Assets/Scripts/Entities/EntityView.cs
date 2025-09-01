using System;
using Entities.Interfaces;
using UnityEngine;
using Utilities;

namespace Entities
{
    public abstract class EntityView : MonoBehaviour,ILook ,ISound ,IAnimate
    {
        [SerializeField] protected Animator animator;
        [SerializeField] protected EntityManager manager;
        [SerializeField] protected SpriteRenderer sprite;
        [SerializeField]private BlinkValues blinkValues; 
        private SpriteEffects _blink;

        public SpriteRenderer Sprite { get => sprite; }

        public abstract void LookDir(Vector2 dir);
        public abstract void LookDirInsta(Vector2 dir);


        #region ISound

            public event Action<string, string> Sounds;
            public virtual void PlaySound(string soundName, string entityName)
            {
                Sounds?.Invoke(soundName,entityName);
            }

        #endregion

        #region IAnimate

            public abstract void PlayStateAnimation(StateEnum state);
            public abstract void StopStateAnimation(StateEnum state);

        #endregion




        protected virtual void Start()
        {
            _blink = new SpriteEffects(this);
            if (manager != null)
            {
                manager.HealthComponent.OnDamage += (float a) => _blink.Blink(sprite, blinkValues.amount, blinkValues.frequency, blinkValues.blinkActive);
            }
        }
    }
}