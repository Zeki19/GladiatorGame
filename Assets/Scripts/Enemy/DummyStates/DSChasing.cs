using UnityEngine;

public class DSChasing<T> : DummyBase<T>
{
    private T _transitionToAttack;
    private float _chaseSpeed;
    private float _attackRange;

    public DSChasing(T transitionToAttack, float chaseSpeed = 2f, float attackRange = 2f)
    {
        _transitionToAttack = transitionToAttack;
        _chaseSpeed = chaseSpeed;
        _attackRange = attackRange;
    }

    public override void Enter()
    {
        base.Enter();
        // Optional: _animator?.Play("Run");
    }

    public override void Execute()
    {
        base.Execute();

        if (_target != null)
        {
            Vector2 toTarget = _target.position - _selfTransform.position;
            
            if (toTarget.magnitude <= _attackRange)
            {
                StateMachine.Transition(_transitionToAttack);
                return;
            }
            
            Vector2 moveDir = toTarget.normalized;
            
            _selfTransform.position += (Vector3)(moveDir * (_chaseSpeed * Time.deltaTime));
        }
    }
}
