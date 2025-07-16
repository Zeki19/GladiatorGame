using Entities;
using UnityEngine;

namespace Enemies
{
    public class EnemyView : EntityView
    {
        [SerializeField] protected float rotationSpeed;
        public override void LookDir(Vector2 dir){}
        public override void LookDirInsta(Vector2 dir)
        {
            throw new System.NotImplementedException();
        }

        public override void PlayStateAnimation(StateEnum state){}
        public override void StopStateAnimation(StateEnum state)
        {
            throw new System.NotImplementedException();
        }
    }
}
