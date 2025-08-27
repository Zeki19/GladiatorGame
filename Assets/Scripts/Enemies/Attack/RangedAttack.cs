using System.Collections;
using Enemies;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedAttack", menuName = "Attacks/Ranged Attack")]
public class RangedAttack : BaseAttack
{
    public float projectileSpeed;
    public float maxRange;
    public GameObject projectilePrefab;

    public override void ExecuteAttack()
    {
        // Implement ranged attack logic
        Debug.Log($"Executing ranged attack: {attackName}");
    }

    protected override IEnumerator Attack()
    {
        throw new System.NotImplementedException();
    }
}