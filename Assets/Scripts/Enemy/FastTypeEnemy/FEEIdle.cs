using UnityEngine;

public class FEEIdle<T> : FastEnemyBase<T>
{
    private T _transitionToSearch;
    private float _idleDuration = 3f;
    private float _timer = 0f;

    public FEEIdle(Transform self, Transform player, T transitionToSearch) 
    {
        _transitionToSearch = transitionToSearch;
    }

    public override void Enter()
    {
        Debug.Log("IDLE");
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
    
    public override void Exit()
    {
        Debug.Log("exiting IDLE");
    }

    private bool CanSeePlayer()
    {
        var los = new LineOfSight2D();
        return los.CheckRange(_selfTransform, _target, 5f) &&
        los.CheckAngle(_selfTransform, _target, 90f) &&
               los.CheckView(_selfTransform, _target, LayerMask.GetMask("Obstacle")); 
    }
}