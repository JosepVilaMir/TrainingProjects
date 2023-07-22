using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    [SerializeField] private enum EnemyState { GoToBase, AttackBase, ChasePlayer, AttackPlayer }
    [SerializeField] private EnemyState currentState;
    [SerializeField] private Sight sightSensor;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private float lastShootTime;
    [SerializeField] private AudioSource shootSound;

    private Transform baseTransform;
    private NavMeshAgent agent;

    [SerializeField] private float baseAttackDistance;
    [SerializeField] private float playerAttackDistance;

    [SerializeField] private ParticleSystem muzzleEffect;

    private void Awake()
    {
        baseTransform = GameObject.Find("BaseDamagePoint").transform;
        agent = GetComponentInParent<NavMeshAgent>();
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
        agent.isStopped = false;
        agent.SetDestination(baseTransform.position);

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
    void AttackBase() 
    { 
        agent.isStopped = true;
        LookTo(baseTransform.position);
        Shoot();

    }
    void ChasePlayer() 
    {
        agent.isStopped = false;

        if(sightSensor.GetDetectedObject() == null)
        {
            currentState = EnemyState.GoToBase;
            return;
        }

        agent.SetDestination(sightSensor.GetDetectedObject().transform.position);

        float distanceToPlayer = Vector3.Distance(transform.position, sightSensor.GetDetectedObject().transform.position);

        if(distanceToPlayer <= playerAttackDistance)
        {
            currentState = EnemyState.AttackPlayer;
        }
    }
    void AttackPlayer() 
    {
        agent.isStopped = true;

        if(sightSensor.GetDetectedObject() == null)
        {
            currentState = EnemyState.GoToBase;
            return;
        }

        LookTo(sightSensor.GetDetectedObject().transform.position);
        Shoot();

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

    void Shoot()
    {
        var timeSinceLastShoot = Time.time - lastShootTime;
        if (timeSinceLastShoot > fireRate) 
        {
            lastShootTime = Time.time;
            Instantiate(bulletPrefab, transform.position, transform.rotation);
            muzzleEffect.Play();
            shootSound.Play();
        }
    }

    void LookTo(Vector3 targetPosition)
    {
        Vector3 directionToPosition = Vector3.Normalize(targetPosition - transform.parent.position);
        directionToPosition.y = 0;
        transform.parent.forward = directionToPosition;
    }
}
