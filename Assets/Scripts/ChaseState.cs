using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{
    private StatePatternEnemy enemy;
    
    public Transform lastHit;

    public ChaseState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Look();
        Chase();
    }

    public void OnTriggerEnter(Collider other)
    {
    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;
    }

    public void toChaseState()
    {
        //Ei voi käyttää
    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;
    }

    void Look()
    {

        RaycastHit hit;
        Vector3 enemyToTarget = enemy.chaseTarget.position - enemy.eyes.transform.position;

        Debug.DrawRay(enemy.eyes.transform.position, enemyToTarget, Color.red, 1f);

        if (Physics.Raycast(enemy.eyes.transform.position, enemyToTarget, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        {
            lastHit = hit.transform;
            lastHit.position = hit.transform.position;
            enemy.chaseTarget = hit.transform;
            Debug.Log(lastHit.position);
        }
        else
        {
            GameObject lastHitObject = new GameObject();
            lastHitObject.gameObject.tag = "LastPosition";
            lastHitObject.transform.position = lastHit.position;
            toTrackingState();
        }
    }

    void Chase()
    {
        enemy.indicator.material.color = Color.red;
        enemy.navMeshAgent.destination = enemy.chaseTarget.position;
        enemy.navMeshAgent.isStopped = false;
    }

    public void toTrackingState()
    {
        enemy.currentState = enemy.trackingState;
    }
}
