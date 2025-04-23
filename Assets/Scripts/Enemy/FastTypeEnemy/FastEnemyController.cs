using System.Collections.Generic;
using UnityEngine;
using static Unity.Cinemachine.CinemachineTriggerAction.ActionSettings;
public class FastEnemyController : MonoBehaviour
{
    [SerializeField] private float viewRange = 5f;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private float movSpeed = 5f;
    [SerializeField] private LayerMask _OBS;
    FSM<StateEnum> _fsm;
    private LineOfSight2D _LoS;
    [SerializeField] private Transform _target;

    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float idleDuration = 2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Transform target;
    private Rigidbody2D _rigidbody;
    private void Awake()
    {
        _LoS = new LineOfSight2D();
    }

    private void Start()
    {
        InitializedSteering();
        InitializedFSM();
        InitializedTree();
    }

    private void Update()
    {
        if (_LoS.CanSee(transform, _target, viewRange, viewAngle, _OBS))
        {
            _fsm
        }
    }

    private void FixedUpdate()
    {
        _fsm.OnFixExecute();
    }
    void InitializedSteering()
    {
        var seek = new Seek(_model.transform, target.transform);
        var flee = new Flee(_model.transform, target.transform);
        var pursuit = new Pursuit(_model.transform, target, 0, timePrediction);
        var evade = new Evade(_model.transform, target, 0, timePrediction);
        _steering = pursuit;
    }
    void InitializedFSM()
    {
        _fsm = new FSM<StateEnum>();
        var look = GetComponent<ILook>();

        var idle = new NPCSIdle<StateEnum>();
        var attack = new NPCSAttack<StateEnum>();
        var chase = new NPCSSteering<StateEnum>(_steering);
        var goZone = new NPCSChase<StateEnum>(zone);

        var stateList = new List<PSBase<StateEnum>>();
        stateList.Add(idle);
        stateList.Add(attack);
        stateList.Add(chase);
        stateList.Add(goZone);

        ition(StateEnum.Spin, attack);
        goZone.AddTransition(StateEnum.Idle, idle);

        for (int i = 0; i < stateList.Count; i++)
        {
            stateList[i].Initialize(_model, look, _model);
        }
        _fsm.SetInit(idle);
    }

    void InitializedTree()
    {
        var idle = new ActionNode(() => _fsm.Transition(StateEnum.Idle));
        var attack = new ActionNode(() => _fsm.Transition(StateEnum.Spin));
        var chase = new ActionNode(() => _fsm.Transition(StateEnum.Chase));
        var goZone = new ActionNode(() => _fsm.Transition(StateEnum.GoZone));

        var qCanAttack = new QuestionNode(QuestionCanAttack, attack, chase);
        var qGoToZone = new QuestionNode(QuestionGoToZone, goZone, idle);
        var qTargetInView = new QuestionNode(QuestionTargetInView, qCanAttack, qGoToZone);

        _root = qCanAttack;
    }
    bool QuestionCanAttack()
    {
        return Vector3.Distance(_model.Position, target.position) <= _model.attackRange;
    }
    bool QuestionGoToZone()
    {
        return Vector3.Distance(_model.transform.position, zone.transform.position) > 0.25f;
    }
    bool QuestionTargetInView()
    {
        if (target == null) return false;
        return _LoS.LOS(target.transform);
    }

}
    
    //WAYPOINT
    //PATROL
    //PURSUE - roulette

    //Dividimos perseguir con el attack a base del arbol de decisiones
    //guardar en el model ese comando al ser parte del ataque
    //para sacar la distancia de escape seria con un float
    //fijar en el arbol de decisiones el rango de escape
    //usamos el checkeo del rango de escape en vez de alternar estados





