using UnityEngine;


public class FastEnemyController : MonoBehaviour
{
    [SerializeField] private float viewRange = 5f;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private LayerMask _OBS;
    private FSM<StateEnum> _fsm;

    private LineOfSight2D _LoS;
    public Transform Player;
    public Vector3 LastKnownPlayerPos;

    //WAYPOINT
    //PATROL
    //PURSUE - roulette

    //Dividimos perseguir con el attack a base del arbol de decisiones
    //guardar en el model ese comando al ser parte del ataque
    //para sacar la distancia de escape seria con un float
    //fijar en el arbol de decisiones el rango de escape
    //usamos el checkeo del rango de escape en vez de alternar estados
    

    private void Awake()
    {
        _LoS = new LineOfSight2D();

        //change
        Player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
    private void Start()
    {
        var idleState = new FEEIdle<StateEnum>(transform, Player, StateEnum.GoZone);
        _fsm = new FSM<StateEnum>(idleState);
    }
    private void Update()
    {
        Vector2 dirToPlayer = Player.position - transform.position;
        _fsm.OnExecute(dirToPlayer);
    }
    public bool CanSeePlayer()
    {
        return _LoS.CheckRange(transform, Player, viewRange) &&
               _LoS.CheckAngle(transform, Player, viewAngle) &&
               _LoS.CheckView(transform, Player, _OBS);
    }
    public void Transition(StateEnum input)
    {
        _fsm.Transition(input);
    }
}

