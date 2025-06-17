namespace Enemies.BinaryTree.QuestionFunctions
{
    public enum QuestionEnum
    {
        PlayerIsInAStraightLine,
        DistanceToWall,
        IsFarToPoint1, //Hound: FarFromCamp
        IsFarToPoint2, //Hound: FarFromLimit
        IsNearToPoint1,
        IsNearToPoint2,
        IsInIdleState,
        IsInPatrolState,
        IsInChaseState,
        IsInSearchState,
        IsInAttackState,
        IsPlayerAlive,
        IsTired, //Hound: isTired
        IsRested, //Hound: isRested
        IsAttackOnCd, //hound: isAttackOnCd
        FinishedSearching,

        WasLastStateAttack, 
        DidAttackMiss,
        IsCurrentlyAttacking,
        IsInShortRange,
        IsInMidRange,
        IsInLongRange,
        IsInPhase1,
        IsJumpingBackwards,
        FinishedAttacking,
        CanLongAttack
    }
}
