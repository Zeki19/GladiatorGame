using System.Collections;
using Enemies;
using UnityEngine;

[CreateAssetMenu(fileName = "RunningAttack", menuName = "Attacks/RunningAttack")]
public class RunningAttack : BaseAttack
{
    public float distance;
    public float speed;

    public override void ExecuteAttack()
    {
        Move.StopMovement();
        CoroutineRunner.StartCoroutine(Attack());
    }

    protected override IEnumerator Attack()
    {
        Weapon.SetActive(true);
        var user = Move as MonoBehaviour;
        Move.Dash(user.transform.up, speed,distance);
        Status.SetStatus(StatusEnum.Dashing, true);
        while (Status.GetStatus(StatusEnum.Dashing)) 
        {
            yield return 0;
        }
        FinishAttack();
    }
}