using UnityEngine;
using Utilities;

namespace Entities
{
    public abstract class EntityView : MonoBehaviour, ILook
    {
        [SerializeField] protected Animator animator;
        [SerializeField] protected EntityManager manager;
        [SerializeField] protected SpriteRenderer sprite;
        private SpriteEffects _blink;
        [SerializeField]private BlinkValues blinkValues; 
    

        public abstract void LookDir(Vector2 dir);
        public abstract void PlayStateAnimation(StateEnum state);
        public abstract void StopStateAnimation(StateEnum state);

        protected virtual void Start()
        {
            _blink = new SpriteEffects(this);
            manager.HealthComponent.OnDamage +=(float a)=> _blink.Blink(sprite,blinkValues.amount,blinkValues.frequency,blinkValues.blinkActive);
        }
    }
}