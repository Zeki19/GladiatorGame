using System.Collections.Generic;
using UnityEngine;

public class FastEnemyController : MonoBehaviour
{
    public Rigidbody target;
    public Transform zone;
    public float timePrediction;
    FSM<StateEnum> _fsm;
    FastEnemyModel _model;
    LineOfSightMono _LoS;
    ITreeNode _root;
    ISteering _steering;
    private void Awake()
    {
        _model = GetComponent<FastEnemyModel>();
        _LoS = GetComponent<LineOfSightMono>();
    }

    private void Start()
    {
        InitializedSteering();
        InitializedFSM();
        InitializedTree();
    }

    void Update()
    {
        if (target != null)
        {
            _fsm.OnExecute();
            _root.Execute();
        }
    }

    private void FixedUpdate()
    {
        _fsm.OnFixExecute();
    }
    void InitializedSteering()
    {

    }
    void InitializedFSM()
    {
        _fsm = new FSM<StateEnum>();
    }

    void InitializedTree()
    {
        
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





