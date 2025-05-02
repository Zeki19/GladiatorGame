using UnityEngine;

[CreateAssetMenu(fileName = "DistanceToPlayerSO", menuName = "Scriptable Objects/Questions/DistanceToPlayer")]
public class DistanceToPlayerSO : QuestionBase
{
    [SerializeField] Vector2 playerPosition;
    public override bool Execute()
    {
        //return Vector2.Distance(_model.Position, playerPosition) <= _model.AttackRange;
        Debug.LogError("Not Implemented");
        return true;
    }
}
