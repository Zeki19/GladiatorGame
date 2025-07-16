using UnityEngine;

namespace Entities.Interfaces
{
    public interface ILook
    {
        void LookDir(Vector2 dir);
        public void LookDirInsta(Vector2 dir);
    }
}
