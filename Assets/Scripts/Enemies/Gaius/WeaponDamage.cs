using Enemies.Gaius;
using Entities;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] GaiusController GaiusController;
    [SerializeField] GaiusModel GaiusModel;
    private IStatus status;
    public AttackManager Attack;
    private void Awake()
    {
        status = GaiusController;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") 
        {
            return;
        }
        status.SetStatus(StatusEnum.AttackMissed,false);
        GaiusModel.AttackTarget(collision.transform, Attack.GetAttackDamage(GaiusController.currentAttack));
        //switch (GaiusController.currentAttack)
        //{
        //    case AttackType.Lunge:
        //        status.SetStatus(StatusEnum.AttackMissed,false);
        //        GaiusModel.AttackTarget(collision.transform, GaiusController.stats.mediumDamage);
        //        break;
        //    case AttackType.Swipe:
        //        status.SetStatus(StatusEnum.AttackMissed,false);
        //        GaiusModel.AttackTarget(collision.transform, GaiusController.stats.shortDamage);
        //        break;
        //    case AttackType.Super:
        //        status.SetStatus(StatusEnum.AttackMissed,false);
        //        GaiusModel.AttackTarget(collision.transform, GaiusController.stats.longDamage);
        //        break;
        //    default:
        //        Debug.Log("PIFIASTE CHAMACO");
        //        break;
        //}
        
    }
}
