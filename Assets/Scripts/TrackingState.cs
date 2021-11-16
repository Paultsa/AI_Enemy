using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingState : IEnemyState
{

    private StatePatternEnemy enemy;

    GameObject lastHitObject;

    public TrackingState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
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
    }

    public void ToPatrolState()
    {
    }

    public void toTrackingState()
    {
    }

    public void UpdateState()
    {
        Find();
    }

    void Find()
    {
        lastHitObject = GameObject.FindGameObjectWithTag("LastPosition");
        enemy.chaseTarget = lastHitObject.transform;
        enemy.navMeshAgent.destination = enemy.chaseTarget.position;
        Debug.Log("Last position: " + lastHitObject.transform.position);
        Debug.Log("Enemy position: " + enemy.transform.position);
        if(enemy.transform.position.x == lastHitObject.transform.position.x && enemy.transform.position.z == lastHitObject.transform.position.z)
        {
            enemy.DestroyLastPosition(lastHitObject);
            ToAlertState();
        }
    }
}
