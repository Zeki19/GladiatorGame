using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "RunningAttack", menuName = "Weapons/RunningAttack")]
public class RunningAttack : EnemyBaseAttack
{
    public float distance;
    public float speed;

    public override void ExecuteAttack()
    {
        Move.StopMovement();
        CoroutineRunner.StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        float time = 0;
        Weapon.SetActive(true);
        var enemy = Move as MonoBehaviour;
        Move.Dash(enemy.transform.up, speed,distance);
        Status.SetStatus(StatusEnum.Dashing, true);
        while (Status.GetStatus(StatusEnum.Dashing)) 
        {
            yield return 0;
        }
        FinishAttack();
    }
}