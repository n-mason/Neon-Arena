using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Followed tutorial at: https://www.youtube.com/watch?v=UjkSFoLxesw

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //public GameObject newEnemy;

    public ParticleSystem deathEffect;
    

    public float health = 100f;
    public float enemydamagetoplayer = 40f;

    public static bool OneShotBaught = false;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked = false;
    //public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;

        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check sight/attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        //Debug.Log("Chasing The Player!");
    }
    private void AttackPlayer()
    {
        //Stop Enemy From Moving
        agent.SetDestination(transform.position);

        transform.LookAt(player);
        //Debug.Log("Looking At The Player!");

        if (!alreadyAttacked)
        {
            //Attack code will go here
            Debug.Log("Attacking The Player!");
            GameObject playerObject = GameObject.FindWithTag("Player");
            playerObject.GetComponent<GameOver>().PlayerTakeDamage(enemydamagetoplayer);

            //Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            //rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public int TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Die();
            return 1;
            // Respawn();
        }
        else
        {
            return 0;
        }
    }

    private void Die()
    {
        Vector3 up = new Vector3(0, 2, 0);
        Vector3 new_pos = transform.position + up;
        Instantiate(deathEffect, new_pos, Quaternion.identity);
        
        Destroy(gameObject);
    }

    

    /*
    private void Respawn()
    {
        Instantiate(newEnemy, new Vector3(1, 1, 1), Quaternion.identity);
    }
    */
}
