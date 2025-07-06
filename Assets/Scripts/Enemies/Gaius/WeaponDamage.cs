using Enemies.Gaius;
using Enemies.Hounds.States;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] GaiusController GaiusController;
    [SerializeField] GaiusModel GaiusModel;
    GaiusStateShortAttack<StateEnum> shortAttack;
    private void Awake()
    {
        shortAttack = GaiusController._shortAttackState as GaiusStateShortAttack<StateEnum>;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") 
        {
            return;
        }
        switch (GaiusController.currentAttack)
        {
            case AttackType.Lunge:
                GaiusController.didAttackMiss = false;
                GaiusModel.AttackTarget(collision.transform, GaiusController.stats.mediumDamage);
                break;
            case AttackType.Swipe:
                GaiusController.didAttackMiss = false;
                GaiusModel.AttackTarget(collision.transform, GaiusController.stats.shortDamage);
                break;
            default:
                Debug.Log("PIFIASTE CHAMACO");
                break;
        }
        
    }
}
