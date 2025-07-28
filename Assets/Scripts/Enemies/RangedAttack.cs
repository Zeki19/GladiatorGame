using UnityEngine;

[CreateAssetMenu(fileName = "RangedAttack", menuName = "Attacks/Ranged Attack")]
public class RangedAttack : EnemyBaseAttack
{
    public float projectileSpeed;
    public float maxRange;
    public GameObject projectilePrefab;

    public override void ExecuteAttack()
    {
        // Implement ranged attack logic
        Debug.Log($"Executing ranged attack: {attackName}");
    }
}