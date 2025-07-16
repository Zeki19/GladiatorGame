namespace Entities.Interfaces
{
    public interface IAnimate
    {
        public void PlayStateAnimation(StateEnum state);
        public void StopStateAnimation(StateEnum state);
    }
}