using UnityEngine;

public class DummyModel : PlayerModel
{
    
    public float speed;
    public float attackRange;
    public LayerMask enemyMask;

    public override void Attack()
    {
        base.Attack();
        
        //Probando el ataque del Dummy como lo hizo el profe en DecisionTree
        var colliders = Physics.OverlapSphere(Position, attackRange, enemyMask);
        foreach (var t in colliders)
        {
            Destroy(t.gameObject);
        }
    }
}
