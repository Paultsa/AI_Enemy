using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePatternEnemy : MonoBehaviour
{
    public float searchTurnSpeed;
    public float searchingDuration;
    public float sightRange;
    public Transform[] waypoints;
    public Transform eyes;
    public MeshRenderer indicator;

    [HideInInspector]
    public Transform chaseTarget;
    [HideInInspector]
    public IEnemyState currentState;
    [HideInInspector]
    public PatrolState patrolState;
    [HideInInspector]
    public AlertState alertState;
    [HideInInspector]
    public ChaseState chaseState;
    [HideInInspector]
    public TrackingState trackingState;


    [HideInInspector]
    public UnityEngine.AI.NavMeshAgent navMeshAgent;

    void Awake()
    {
        chaseState = new ChaseState(this);
        patrolState = new PatrolState(this);
        alertState = new AlertState(this);
        trackingState = new TrackingState(this);
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Start()
    {
        currentState = patrolState;
    }
    void Update()
    {
        currentState.UpdateState();
        Debug.Log(currentState);
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }

    public void DestroyLastPosition(GameObject lastPosition)
    {
        Destroy(lastPosition);
    }

    /*
     Tee neljäs tila trackingState. Etsii pelaajan edellisen paikan ja siirtyy siellä AlertStateen.
     */
}
