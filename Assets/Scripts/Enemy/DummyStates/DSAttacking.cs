using UnityEngine;

public class DSAttacking<T> : DummyBase<T>
{
    private T _transitionToChase;
    private float _attackRange = 2f;
    private float _attackCooldown = 1f;
    private float _timer;

    public DSAttacking(T transitionToChase, float attackRange = 2f, float attackCooldown = 1f)
    {
        _transitionToChase = transitionToChase;
        _attackRange = attackRange;
        _attackCooldown = attackCooldown;
    }

    public override void Enter()
    {
        base.Enter();
        _timer = _attackCooldown;
        
        _target.GetComponent<IHealth>()?.TakeDamage(10);
    }

    public override void Execute(Vector2 direction)
    {
        base.Execute(direction);

        _timer -= Time.deltaTime;

        if (_target == null) return;

        float distance = Vector2.Distance(_selfTransform.position, _target.position);

        if (distance > _attackRange)
        {
            StateMachine.Transition(_transitionToChase);
        }
        else if (_timer <= 0f)
        {
            StateMachine.Transition(_transitionToChase);
        }
    }
}
