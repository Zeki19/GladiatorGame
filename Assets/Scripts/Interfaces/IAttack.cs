using System;

namespace Interfaces
{
    public interface IAttack
    {
        void Attack();
        Action OnAttack { get; set; }
    }
}
