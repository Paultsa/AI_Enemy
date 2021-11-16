using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private readonly StatePatternEnemy enemy;

    private int nextWaypoint;

    public PatrolState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Patrol();
        Look();
    }

    public void OnTriggerEnter(Collider other)
    {   
        if(other.gameObject.CompareTag("Player"))
        {
            ToAlertState();
        }
    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;
    }

    public void toChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }

    public void ToPatrolState()
    {
        //Ei voi käyttää
    }

    void Patrol()
    {
        enemy.indicator.material.color = Color.green;
        enemy.navMeshAgent.destination = enemy.waypoints[nextWaypoint].position;
        enemy.navMeshAgent.isStopped = false;

        if(enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance && !enemy.navMeshAgent.pathPending)
        {
            nextWaypoint = (nextWaypoint + 1) % enemy.waypoints.Length;
        }
    }

    void Look()
    {
        Debug.DrawRay(enemy.eyes.transform.position, enemy.eyes.transform.forward * enemy.sightRange, Color.green, 1f);
        RaycastHit hit;
        if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player")) {
            enemy.chaseTarget = hit.transform;
            toChaseState();
        }
    }

    public void toTrackingState()
    {
    }
}
