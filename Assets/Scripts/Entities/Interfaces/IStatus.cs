using Enemies;
using Unity.Behavior;

namespace Entities
{
    public interface IStatus
    {
    public bool GetStatus(StatusEnum status);

    public void SetStatus(StatusEnum status, bool value);
    }
}
[BlackboardEnum]
public enum StatusEnum
{
    AttackMissed=0,
    Attacking=1,
    Dashing=2,
    OnMeleeShortCD=3,
    OnMeleeLongtCD=4,
    OnMiddleCD=5,
    OnLongCD=6,
    isMeleeLock=7,
    IsFleeing=8,
    isInBack=9,
    FinishedSearching=10,
    SawThePlayer=11,
    ChargeNow=12,
    WallBreaker=13
}