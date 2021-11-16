using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : IEnemyState
{
    private StatePatternEnemy enemy;

    private float searchTimer;

    public AlertState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Look();
        Search();
    }

    public void OnTriggerEnter(Collider other)
    {
    }

    public void ToAlertState()
    {
        // Ei voi käyttää
    }

    public void toChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;
        searchTimer = 0f;
        enemy.indicator.material.color = Color.green;
    }


    void Look()
    {
        Debug.DrawRay(enemy.eyes.transform.position, enemy.eyes.transform.forward * enemy.sightRange, Color.yellow, 1f);
        RaycastHit hit;
        if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        {
            enemy.chaseTarget = hit.transform;
            toChaseState();
            
        }
    }

    void Search()
    {
        enemy.indicator.material.color = Color.yellow;
        enemy.navMeshAgent.isStopped = true;
        enemy.transform.Rotate(0, enemy.searchTurnSpeed * Time.deltaTime, 0);
        searchTimer += Time.deltaTime;
        if(searchTimer >= enemy.searchingDuration)
        {
            ToPatrolState();
        }
    }

    public void toTrackingState()
    {
    }
}
