using UnityEngine;

public class MissAttackHandler
{
    private int TotalMissAttacks;
    private int CurrentMissAttacks;

    public int TotalMissAttacksCount => TotalMissAttacks;
    public int CurrentMissAttacksCount => CurrentMissAttacks;
    public void AddMissAttack()
    {
        TotalMissAttacks++;
        CurrentMissAttacks++;
        Debug.Log(CurrentMissAttacks);
    }

    public void ResetMissAttacks()
    {
        CurrentMissAttacks = 0;
    }
}