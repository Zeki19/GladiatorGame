using Entities;

namespace Enemies
{
    public interface ITarget
    {
        public EntityManager GetTarget();
        public void SetTarget(EntityManager target);
    }
}