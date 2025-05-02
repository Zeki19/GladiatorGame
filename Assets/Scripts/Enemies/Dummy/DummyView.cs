using UnityEngine;

namespace Dummy
{
    public class DummyView : EntityView, ILook
    {
        public override void LookDir(Vector2 dir)
        {
            throw new System.NotImplementedException();
        }

        public override void PlayStateAnimation(StateEnum state)
        {
            throw new System.NotImplementedException();
        }
    }
}