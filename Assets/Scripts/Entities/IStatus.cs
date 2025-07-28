using Enemies;

namespace Entities
{
    public interface IStatus
    {
    public bool GetStatus(StatusEnum status);

    public void SetStatus(StatusEnum status, bool value);
    }
}
public enum StatusEnum
{
    AttackMissed=0,
    Attacking=1,
    Dashing=2,
}