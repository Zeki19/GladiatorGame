using UnityEngine;

public class FEEIdle<T> : FastEnemyBase<T>
{
    private T _goPatrol;
    private float _idleTimer;
    private float _idleDuration;
    private State<T> _nextState;
    FSM<T> _fsm;

    public override void Initialize(params object[] args)
    {
        base.Initialize(args);

        _idleDuration = (float)args[4];
        _nextState = args[5] as State<T>;
    }

    public override void Enter()
    {
        base.Enter();
        _idleTimer = 2f;
    }

    public override void Execute(Vector2 direction)
    {
        _idleTimer += Time.deltaTime;

        if (_idleTimer >= _idleDuration)
        {
            StateMachine.Transition(_goPatrol);
        }

    }
}
        //public override void Execute(Vector2 direction)
        //{
        //    _timer += Time.deltaTime;

        //    if (CanSeePlayer())
        //    {
        //        StateMachine.Transition(_transitionToSearch);
        //        return;
        //    }

        //    if (_timer >= _idleDuration)
        //    {
        //        StateMachine.Transition(_transitionToSearch);
        //    }
        //}

        //sacar direccion, los estados no deciden nada, LO DECIDE ARBOL DE DIRECCION (TODO LO QUE ES TRANSITION)
        // GUARDAR ESTADO EN VARIABLE Y SACO LA VARIABLE CON EL ARBOL
        //TENER REFERENCIA AL PLAYER CON UNA FLAG
        //SI TENEMOS LA RULETA VA A MEDIR CHANCES A TRAVES DE FRAMES EN EL UPDATE, MEJOR MARCARLO COMO EVENTO.


    