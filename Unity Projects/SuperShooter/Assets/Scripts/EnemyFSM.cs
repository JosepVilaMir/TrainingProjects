using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    [SerializeField] private enum EnemyState { GoToBase, AttackBase, ChasePlayer, AttackPlayer }
    [SerializeField] private EnemyState currentState;
    [SerializeField] private Sight sightSensor;

    private Transform baseTransform;

    [SerializeField] private float baseAttackDistance;
    [SerializeField] private float playerAttackDistance;

    private void Awake()
    {
        baseTransform = GameObject.Find("BaseDamagePoint").transform;
    }

    private void Update()
    {
        if (currentState == EnemyState.GoToBase) { GoToBase();}
        else if (currentState == EnemyState.AttackBase) { AttackBase();}
        else if (currentState == EnemyState.ChasePlayer) { ChasePlayer();}
        else { AttackPlayer();}
    }

    void GoToBase() 
    { 
        if(sightSensor.GetDetectedObject() != null) 
        {
            currentState = EnemyState.ChasePlayer;
        }

        float distanceToBase = Vector3.Distance(transform.position, baseTransform.position);

        if(distanceToBase < baseAttackDistance)
        {
            currentState = EnemyState.AttackBase;
        }
    }
    void AttackBase() { print("AttackBase"); }
    void ChasePlayer() 
    { 
        if(sightSensor.GetDetectedObject() == null)
        {
            currentState = EnemyState.GoToBase;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, sightSensor.GetDetectedObject().transform.position);

        if(distanceToPlayer <= playerAttackDistance)
        {
            currentState = EnemyState.AttackPlayer;
        }
    }
    void AttackPlayer() 
    { 
        if(sightSensor.GetDetectedObject() == null)
        {
            currentState = EnemyState.GoToBase;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, sightSensor.GetDetectedObject().transform.position);

        if(distanceToPlayer > playerAttackDistance * 1.1f) 
        {
            currentState = EnemyState.ChasePlayer;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, playerAttackDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, baseAttackDistance);
    }
}
