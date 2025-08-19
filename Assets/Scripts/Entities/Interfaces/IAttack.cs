using System;

namespace Entities.Interfaces
{
    public interface IAttack
    {
        Action OnAttack { get; set; }
    }
}
